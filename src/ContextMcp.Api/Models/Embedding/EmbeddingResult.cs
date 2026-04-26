using System.Collections.Immutable;

namespace ContextMcp.Api.Models.Embedding
{
    public sealed class EmbeddingResult
    {
        public bool IsSuccess { get; }
        public EmbeddingError? Error { get; }


        /// <summary>Multiple embeddings (GenerateBatchEmbeddingsAsync).</summary>
        public IReadOnlyList<ImmutableArray<float>>? Embeddings { get; }

        /// <summary>Token usage reported by the provider.</summary>
        public TokenUsage? TokenUsage { get; init; }

        private EmbeddingResult(
            IReadOnlyList<ImmutableArray<float>> embeddings, TokenUsage? tokenUsage)
        {
            IsSuccess = true;
            Embeddings = embeddings;
            TokenUsage = tokenUsage;
        }

        private EmbeddingResult(EmbeddingError error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static EmbeddingResult Success(IReadOnlyList<ImmutableArray<float>> embeddings, TokenUsage? tokenUsage)
            => new(embeddings, tokenUsage);

        public static EmbeddingResult Failure(EmbeddingError error)
            => new(error);
    }

    public sealed record TokenUsage(int PromptTokens, int TotalTokens);
}
