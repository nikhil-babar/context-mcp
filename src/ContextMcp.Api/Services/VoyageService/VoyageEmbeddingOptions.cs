namespace ContextMcp.Api.Services.VoyageService
{

    /// <summary>
    /// Configuration options for the Voyage AI embedding provider.
    /// Bind from appsettings.json under "VoyageEmbedding".
    /// </summary>
    public sealed class VoyageEmbeddingOptions
    {
        public const string Section = "VoyageEmbedding";

        /// <summary>Your Voyage AI API key (required).</summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// The Voyage model to use.
        /// Defaults to voyage-3-large which supports up to 2048 output dimensions.
        /// Other options: voyage-3, voyage-3-lite, voyage-code-3, voyage-finance-2
        /// </summary>
        public string Model { get; set; } = "voyage-3-large";

        /// <summary>Base URL for the Voyage API. Override for proxies / testing.</summary>
        public string BaseUrl { get; set; } = "https://api.voyageai.com/v1";

        /// <summary>HTTP timeout per request.</summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>Maximum retries on transient failures (429 / 5xx).</summary>
        public int MaxRetries { get; set; } = 3;

        // ── Per-model dimension caps ──────────────────────────────────────────────
        // https://docs.voyageai.com/docs/embeddings
        private static readonly Dictionary<string, int> ModelMaxDimensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ["voyage-3-large"] = 2048,
            ["voyage-3"] = 1024,
            ["voyage-3-lite"] = 512,
            ["voyage-code-3"] = 2048,
            ["voyage-finance-2"] = 1024,
            ["voyage-law-2"] = 1024,
            ["voyage-multilingual-2"] = 1024,
        };

        // ── Per-model context window (tokens) ─────────────────────────────────────
        private static readonly Dictionary<string, int> ModelMaxTokens = new(StringComparer.OrdinalIgnoreCase)
        {
            ["voyage-3-large"] = 32_000,
            ["voyage-3"] = 32_000,
            ["voyage-3-lite"] = 32_000,
            ["voyage-code-3"] = 32_000,
            ["voyage-finance-2"] = 32_000,
            ["voyage-law-2"] = 16_000,
            ["voyage-multilingual-2"] = 32_000,
        };

        /// <summary>Returns the maximum output dimensions for the configured model.</summary>
        public int GetMaxDimensions() =>
            ModelMaxDimensions.TryGetValue(Model, out var max) ? max : 1024;

        /// <summary>Returns the maximum input tokens for the configured model.</summary>
        public int GetMaxTokens() =>
            ModelMaxTokens.TryGetValue(Model, out var max) ? max : 32_000;
    }
}
