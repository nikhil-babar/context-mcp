using ContextMcp.Api.Models;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models.ToolRequests
{
    [Description("Request payload for the post-knowledge MCP tool: a single knowledge item with type-specific content.")]
    public class PostKnowledgeBaseToolRequest
    {
        [Description("The knowledge base entry to create or update.")]
        [JsonPropertyName("knowledgeBase")]
        public required KnowledgeBase KnowledgeBase { get; set; }
    }
}
    