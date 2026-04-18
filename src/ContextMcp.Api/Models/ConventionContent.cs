using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for convention-type knowledge entries.")]
    public class ConventionContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for convention content.")]
        [JsonPropertyName("type")]
        public override string Type => "convention";

        [Description("Convention rule being defined.")]
        [JsonPropertyName("rule")]
        public string Rule { get; set; }

        [Description("Targets or scopes where the convention applies.")]
        [JsonPropertyName("applies_to")]
        public List<string> AppliesTo { get; set; }

        [Description("Examples that illustrate how the convention should be applied.")]
        [JsonPropertyName("examples")]
        public List<string> Examples { get; set; }

        [Description("Enforcement level of the convention, such as strict or recommended.")]
        [JsonPropertyName("enforcement_level")]
        public string EnforcementLevel { get; set; }
    }
}

