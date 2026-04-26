using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Models.Embedding;
using ContextMcp.Api.Services.VoyageService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ContextMcp.Api.Services.VoyageService;

/// <summary>
/// Voyage AI implementation of <see cref="IEmbeddingService"/>.
/// Register via AddVoyageEmbeddingService() extension.
/// </summary>
public sealed class VoyageEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _voyageClient;
    private readonly VoyageEmbeddingOptions _options;
    private readonly ILogger<VoyageEmbeddingService> _logger;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    public string ProviderName => "Voyage AI";
    public int MaxDimensions => _options.GetMaxDimensions();

    public VoyageEmbeddingService(
        HttpClient http,
        IOptions<VoyageEmbeddingOptions> options,
        ILogger<VoyageEmbeddingService> logger)
    {
        _voyageClient = http;
        _options = options.Value;
        _logger = logger;
    }

    // ── GenerateBatchEmbeddingsAsync ──────────────────────────────────────────

    public async Task<EmbeddingResult> GenerateBatchEmbeddingsAsync(
        IReadOnlyList<string> texts, int dimensions, CancellationToken ct = default)
    {
        if (texts.Count == 0)
            return EmbeddingResult.Failure(new InvalidResponseError("Batch must contain at least one text."));

        var dimensionError = ValidateDimensions(dimensions);
        if (dimensionError is not null)
            return EmbeddingResult.Failure(dimensionError);

        var request = BuildRequest(texts, dimensions);
        return await PostAsync(request, ct, isBatch: true);
    }

    // ── Core HTTP call ────────────────────────────────────────────────────────

    private async Task<EmbeddingResult> PostAsync(
        VoyageEmbedRequest request, CancellationToken ct, bool isBatch = false)
    {
        for (int attempt = 1; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                using var httpReq = new HttpRequestMessage(HttpMethod.Post, "/v1/embeddings");
                httpReq.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _options.ApiKey);
                httpReq.Content = JsonContent.Create(request, options: JsonOpts);

                using var response = await _voyageClient.SendAsync(httpReq, ct);

                if (response.IsSuccessStatusCode)
                    return await ParseSuccessAsync(response, isBatch, ct);

                var error = await ParseErrorAsync(response, ct);

                // Retry on 429 / 5xx
                if (ShouldRetry(response.StatusCode) && attempt < _options.MaxRetries)
                {
                    var delay = GetRetryDelay(response, attempt);
                    _logger.LogWarning("Voyage API transient error (attempt {A}/{M}), retrying in {D}ms. Status={S}",
                        attempt, _options.MaxRetries, delay.TotalMilliseconds, response.StatusCode);
                    await Task.Delay(delay, ct);
                    continue;
                }

                return EmbeddingResult.Failure(error);
            }
            catch (TaskCanceledException) when (ct.IsCancellationRequested)
            {
                throw; // honour cancellation
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "Voyage API request timed out (attempt {A})", attempt);
                if (attempt >= _options.MaxRetries)
                    return EmbeddingResult.Failure(new TransientError("Request timed out.", ex));
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "Network error calling Voyage API (attempt {A})", attempt);
                if (attempt >= _options.MaxRetries)
                    return EmbeddingResult.Failure(new TransientError("Network error contacting Voyage API.", ex));
            }

            await Task.Delay(TimeSpan.FromMilliseconds(500 * attempt), ct);
        }

        return EmbeddingResult.Failure(new TransientError("Max retries exceeded."));
    }

    // ── Response parsing ──────────────────────────────────────────────────────

    private static async Task<EmbeddingResult> ParseSuccessAsync(
        HttpResponseMessage response, bool isBatch, CancellationToken ct)
    {
        var body = await response.Content.ReadFromJsonAsync<VoyageEmbedResponse>(JsonOpts, ct)
            ?? throw new JsonException("Null response body from Voyage API.");

        if (body.Data is null || body.Data.Count == 0)
            return EmbeddingResult.Failure(
                new InvalidResponseError("Voyage API returned no embedding data."));

        var usage = body.Usage is not null
            ? new TokenUsage(body.Usage.TotalTokens, body.Usage.TotalTokens)
            : null;

        var embeddings = body.Data
            .OrderBy(d => d.Index)
            .Select(d => (d.Embedding ?? []).ToImmutableArray())
            .ToList();
        return EmbeddingResult.Success(embeddings, usage);
    }

    private async Task<EmbeddingError> ParseErrorAsync(
        HttpResponseMessage response, CancellationToken ct)
    {
        var rawBody = await response.Content.ReadAsStringAsync(ct);
        var statusCode = (int)response.StatusCode;

        _logger.LogWarning("Voyage API error: status={Status}, body={Body}", statusCode, rawBody);

        VoyageErrorResponse? errorDto = null;
        try { errorDto = JsonSerializer.Deserialize<VoyageErrorResponse>(rawBody, JsonOpts); }
        catch { /* ignore parse failure */ }

        var message = errorDto?.GetMessage() ?? rawBody;

        return response.StatusCode switch
        {
            HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden =>
                new AuthenticationError($"Voyage API authentication failed: {message}"),

            HttpStatusCode.TooManyRequests =>
                new ResourceExhaustedError(
                    $"Voyage API rate limit exceeded: {message}",
                    GetRetryAfterHeader(response)),

            HttpStatusCode.UnprocessableEntity =>
                ParseUnprocessableError(message),

            HttpStatusCode.BadRequest =>
                ParseBadRequestError(message),

            _ when statusCode >= 500 =>
                new TransientError($"Voyage API server error ({statusCode}): {message}"),

            _ =>
                new InvalidResponseError($"Voyage API returned unexpected status {statusCode}: {message}",
                    statusCode, rawBody),
        };
    }

    private static EmbeddingError ParseUnprocessableError(string message)
    {
        // Voyage returns 422 for token-limit violations
        if (message.Contains("token", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("length", StringComparison.OrdinalIgnoreCase))
            return new TooManyTokensError($"Input exceeds model token limit: {message}");

        return new InvalidResponseError($"Voyage API unprocessable entity: {message}", 422);
    }

    private static EmbeddingError ParseBadRequestError(string message)
    {
        if (message.Contains("token", StringComparison.OrdinalIgnoreCase))
            return new TooManyTokensError(message);

        return new InvalidResponseError($"Voyage API bad request: {message}", 400);
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private EmbeddingError? ValidateInput(string text, int dimensions)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new InvalidResponseError("Input text must not be empty.");

        return ValidateDimensions(dimensions);
    }

    private EmbeddingError? ValidateDimensions(int dimensions)
    {
        if (dimensions <= 0)
            return new InvalidDimensionsError(dimensions);

        var max = _options.GetMaxDimensions();
        if (dimensions > max)
            return new DimensionsExceededError(dimensions, max);

        return null;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private VoyageEmbedRequest BuildRequest(IEnumerable<string> texts, int dimensions)
    {
        var textList = texts.ToList();
        return new VoyageEmbedRequest
        {
            Model = _options.Model,
            Input = textList.Count == 1 ? textList[0] : (object)textList,
            OutputDimension = dimensions,
        };
    }

    private static bool ShouldRetry(HttpStatusCode code) =>
        code == HttpStatusCode.TooManyRequests || (int)code >= 500;

    private static TimeSpan GetRetryDelay(HttpResponseMessage response, int attempt)
    {
        if (response.Headers.RetryAfter?.Delta is { } delta)
            return delta;
        if (response.Headers.RetryAfter?.Date is { } date)
            return date - DateTimeOffset.UtcNow;

        // Exponential back-off: 1s, 2s, 4s …
        return TimeSpan.FromSeconds(Math.Pow(2, attempt - 1));
    }

    private static TimeSpan? GetRetryAfterHeader(HttpResponseMessage response)
    {
        if (response.Headers.RetryAfter?.Delta is { } delta) return delta;
        if (response.Headers.RetryAfter?.Date is { } date) return date - DateTimeOffset.UtcNow;
        return null;
    }
}
