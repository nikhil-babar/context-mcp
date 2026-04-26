namespace ContextMcp.Api.Models.Embedding
{
    /// <summary>
    /// Base class for all embedding service errors.
    /// Match on the concrete type rather than reading a string code.
    /// </summary>
    public abstract class EmbeddingError
    {
        public string Message { get; }
        protected EmbeddingError(string message) => Message = message;
        public override string ToString() => $"[{GetType().Name}] {Message}";
    }

    /// <summary>Requested dimensions exceed the provider maximum.</summary>
    public sealed class DimensionsExceededError : EmbeddingError
    {
        public int Requested { get; }
        public int Maximum { get; }

        public DimensionsExceededError(int requested, int maximum)
            : base($"Requested {requested} dimensions exceeds provider maximum of {maximum}.")
        {
            Requested = requested;
            Maximum = maximum;
        }
    }

    /// <summary>Input text contains more tokens than the provider model allows.</summary>
    public sealed class TooManyTokensError : EmbeddingError
    {
        public int? EstimatedTokens { get; }
        public int? MaxTokens { get; }

        public TooManyTokensError(string message, int? estimatedTokens = null, int? maxTokens = null)
            : base(message)
        {
            EstimatedTokens = estimatedTokens;
            MaxTokens = maxTokens;
        }
    }

    /// <summary>API quota or rate limit has been exhausted.</summary>
    public sealed class ResourceExhaustedError : EmbeddingError
    {
        public TimeSpan? RetryAfter { get; }

        public ResourceExhaustedError(string message, TimeSpan? retryAfter = null)
            : base(message) => RetryAfter = retryAfter;
    }

    /// <summary>Authentication or authorisation failure.</summary>
    public sealed class AuthenticationError : EmbeddingError
    {
        public AuthenticationError(string message) : base(message) { }
    }

    /// <summary>The provider returned an unexpected or malformed response.</summary>
    public sealed class InvalidResponseError : EmbeddingError
    {
        public int? HttpStatusCode { get; }
        public string? RawBody { get; }

        public InvalidResponseError(string message, int? httpStatusCode = null, string? rawBody = null)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            RawBody = rawBody;
        }
    }

    /// <summary>A transient network or timeout error.</summary>
    public sealed class TransientError : EmbeddingError
    {
        public Exception? Inner { get; }
        public TransientError(string message, Exception? inner = null) : base(message) => Inner = inner;
    }

    /// <summary>Dimensions must be a positive integer.</summary>
    public sealed class InvalidDimensionsError : EmbeddingError
    {
        public InvalidDimensionsError(int requested)
            : base($"Dimensions must be a positive integer, got {requested}.") { }
    }
}
