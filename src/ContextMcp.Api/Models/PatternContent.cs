using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for pattern-type knowledge entries.")]
    public class PatternContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for pattern content.")]
        [JsonPropertyName("type")]
        public override string Type => "pattern";

        [Description("Problem that the pattern addresses.")]
        [JsonPropertyName("problem")]
        public required string Problem { get; set; }

        [Description("Context in which the pattern applies.")]
        [JsonPropertyName("context")]
        public required List<string> Context { get; set; }

        [Description("Triggers or conditions that should lead to applying this pattern.")]
        [JsonPropertyName("triggers")]
        public  required List<string> Triggers { get; set; }

        [Description("Recommended steps to apply the pattern.")]
        [JsonPropertyName("solution_steps")]
        public required List<string> SolutionSteps { get; set; }

        [Description("Trade-offs or downsides of using the pattern.")]
        [JsonPropertyName("tradeoffs")]
        public required List<string> Tradeoffs { get; set; }

        [Description("Overall complexity level of the pattern.")]
        [JsonPropertyName("complexity")]
        public required string Complexity { get; set; }
    }
}

