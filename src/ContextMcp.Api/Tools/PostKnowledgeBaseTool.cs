using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Models.ToolRequests;
using ContextMcp.Api.Models.ToolResponses;
using ContextMcp.Api.Utility;
using Microsoft.AspNetCore.Http;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Security.Claims;

namespace ContextMcp.Api.Tools
{
    [McpServerToolType]
    public class PostKnowledgeBaseTool
    {
        private readonly IKnowledgeBaseStore _knowledgeBaseService;
        private readonly IEmbeddingService _embeddingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostKnowledgeBaseTool(IKnowledgeBaseStore knowledgeBaseService, IEmbeddingService embeddingService, IHttpContextAccessor httpContextAccessor)
        {
            _knowledgeBaseService = knowledgeBaseService;
            _embeddingService = embeddingService;
            _httpContextAccessor = httpContextAccessor;
        }

        [McpServerTool(Name = "post-knowledge")]
        [Description(ToolDescriptions.PostKnowledgeBaseToolDescription)]
        public async Task<PostKnowledgeBaseToolResponse> PostKnowledge(
            [Description(ToolDescriptions.PostKnowledgeBaseToolKnowledgeBaseParameterDescription)] PostKnowledgeBaseToolRequest request,
            CancellationToken cnt
         ) 
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) 
            {
                throw new Exception("Unauthorized: User ID not found in context.");
            }
            request.KnowledgeBase.SetUserId(userId);

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
