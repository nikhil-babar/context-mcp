using ContextMcp.Api.Models.Embedding;
using System.Collections.Immutable;

namespace ContextMcp.Api.Interfaces
{
    /// <summary>
    /// Provider-agnostic interface for generating, storing and querying text embeddings.
    /// </summary>
    public interface IEmbeddingService
    {
        /// <summary>
        /// Generates embeddings for multiple texts in a single batch.
        /// </summary>
        Task<EmbeddingResult> GenerateBatchEmbeddingsAsync(IReadOnlyList<string> texts, int dimensions, CancellationToken ct = default);

        /// <summary>
        /// Maximum dimensions supported by this provider.
        /// </summary>
        int MaxDimensions { get; }

        /// <summary>
        /// Name of the underlying provider (e.g. "Voyage", "OpenAI").
        /// </summary>
        string ProviderName { get; }
    }
}
