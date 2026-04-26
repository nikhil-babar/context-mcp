using System.Text.Json.Serialization;

namespace ContextMcp.Api.Services.VoyageService
{
    internal sealed class VoyageEmbedRequest
    {
        [JsonPropertyName("model")]
        public required string Model { get; init; }

        /// <summary>Single string or array of strings.</summary>
        [JsonPropertyName("input")]
        public required object Input { get; init; }

        [JsonPropertyName("input_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputType { get; init; }

        /// <summary>
        /// Output dimensionality. Voyage ignores this for models that don't support
        /// dimension truncation — we validate on our side first.
        /// </summary>
        [JsonPropertyName("output_dimension")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? OutputDimension { get; init; }

        [JsonPropertyName("output_dtype")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OutputDtype { get; init; } = "float";
    }

    internal sealed class VoyageEmbedding
    {
        [JsonPropertyName("object")]
        public string? Object { get; init; }

        [JsonPropertyName("embedding")]
        public List<float>? Embedding { get; init; }

        [JsonPropertyName("index")]
        public int Index { get; init; }
    }

    internal sealed class VoyageUsage
    {
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; init; }
    }

    internal sealed class VoyageEmbedResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; init; }

        [JsonPropertyName("data")]
        public List<VoyageEmbedding>? Data { get; init; }

        [JsonPropertyName("model")]
        public string? Model { get; init; }

        [JsonPropertyName("usage")]
        public VoyageUsage? Usage { get; init; }
    }


    internal sealed class VoyageErrorResponse
    {
        [JsonPropertyName("detail")]
        public string? Detail { get; init; }

        // Some errors wrap in a nested object
        [JsonPropertyName("error")]
        public VoyageErrorDetail? Error { get; init; }

        public string GetMessage() =>
            Detail ?? Error?.Message ?? "Unknown Voyage API error.";
    }

    internal sealed class VoyageErrorDetail
    {
        [JsonPropertyName("message")]
        public string? Message { get; init; }

        [JsonPropertyName("type")]
        public string? Type { get; init; }
    }
}
