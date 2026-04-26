using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Models.ToolRequests;
using ContextMcp.Api.Models.ToolResponses;
using ContextMcp.Api.Utility;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Linq;

namespace ContextMcp.Api.Tools
{
    [McpServerToolType]
    public class PostKnowledgeBaseTool
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;
        private readonly IEmbeddingService _embeddingService;

        public PostKnowledgeBaseTool(IKnowledgeBaseService knowledgeBaseService, IEmbeddingService embeddingService)
        {
            _knowledgeBaseService = knowledgeBaseService;
            _embeddingService = embeddingService;
        }

        [McpServerTool(Name = "post-knowledge")]
        [Description(ToolDescriptions.PostKnowledgeBaseToolDescription)]
        public async Task<PostKnowledgeBaseToolResponse> PostKnowledge(
            [Description(ToolDescriptions.PostKnowledgeBaseToolKnowledgeBaseParameterDescription)] PostKnowledgeBaseToolRequest request,
            CancellationToken cnt
         ) 
        {
            var embeddingResult = await _embeddingService.GenerateBatchEmbeddingsAsync(new[] { request.KnowledgeSummary }, 1024, cnt);
            if (embeddingResult.IsSuccess && embeddingResult.Embeddings != null && embeddingResult.Embeddings.Count > 0)
            {
                var embeddingArray = embeddingResult.Embeddings[0].ToArray();
                request.KnowledgeBase.SetEmbeddingData(embeddingArray, _embeddingService.ProviderName, DateTimeOffset.UtcNow);
            }
            else
            {
                throw new Exception($"Failed to generate embedding: {embeddingResult.Error?.Message}");
            }

            return await _knowledgeBaseService.PostKnowledgeAsync(request, cnt);
        }
    }
}
