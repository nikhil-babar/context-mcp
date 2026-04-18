using System.ComponentModel;

namespace ContextMcp.Api.Models
{
    [Description("Represents a knowledge base entry persisted in the system.")]
    public class KnowledgeBase
    {
        [Description("Optional identifier of the knowledge entry.")]
        public string? Id { get; set; }

        [Description("Short human-readable title for the knowledge entry.")]
        public required string Title { get; set; }

        [Description("Brief summary describing the knowledge entry.")]
        public required string Summary { get; set; }

        [Description("Tags used to categorize and search knowledge entries.")]
        public required List<string> Tags { get; set; }

        [Description("Importance level of the knowledge entry, such as low, medium, or high.")]
        public required string Importance { get; set; }

        [Description("Structured content associated with the knowledge entry.")]
        public required KnowledgeBaseContent Content { get; set; }
    }
}

