using ContextMcp.Api.Models;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models.ToolRequests
{
    [Description("Request payload for the post-knowledge MCP tool: a single knowledge item with type-specific content.")]
    public class PostKnowledgeBaseToolRequest
    {
        [Description("Compact semantic summary of the knowledge base entry for vector similarity search. Include the most important entities, topics, capabilities, domain context, and distinguishing details in concise natural language without filler.")]
        [JsonPropertyName("knowledgeSummary")]
        public required string KnowledgeSummary { get; set; }

        [Description("The knowledge base entry to create or update.")]
        [JsonPropertyName("knowledgeBase")]
        public required KnowledgeBase KnowledgeBase { get; set; }
    }
}
    
