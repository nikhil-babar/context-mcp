using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Models.ToolRequests;
using ContextMcp.Api.Models.ToolResponses;
using ContextMcp.Api.Utility;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ContextMcp.Api.Tools
{
    [McpServerToolType]
    public class PostKnowledgeBaseTool
    {
        private readonly IKnowledgeBaseService _knowledgeBaseService;

        public PostKnowledgeBaseTool(IKnowledgeBaseService knowledgeBaseService)
        {
            _knowledgeBaseService = knowledgeBaseService;
        }

        [McpServerTool(Name = "post-knowledge")]
        [Description(ToolDescriptions.PostKnowledgeBaseToolDescription)]
        public async Task<PostKnowledgeBaseToolResponse> PostKnowledge(
            [Description(ToolDescriptions.PostKnowledgeBaseToolKnowledgeBaseParameterDescription)] PostKnowledgeBaseToolRequest request,
            CancellationToken cnt
         ) 
        {
            return await _knowledgeBaseService.PostKnowledgeAsync(request, cnt);
        }
    }
}
