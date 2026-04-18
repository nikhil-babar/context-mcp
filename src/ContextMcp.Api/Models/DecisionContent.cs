using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for decision-type knowledge entries.")]
    public class DecisionContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for decision content.")]
        [JsonPropertyName("type")]
        public override string Type => "decision";

        [Description("Description of the decision that was made.")]
        [JsonPropertyName("decision")]
        public string Decision { get; set; }

        [Description("Context in which the decision was taken.")]
        [JsonPropertyName("context")]
        public string Context { get; set; }

        [Description("Rationale explaining why the decision was chosen.")]
        [JsonPropertyName("rationale")]
        public string Rationale { get; set; }
    }
}

