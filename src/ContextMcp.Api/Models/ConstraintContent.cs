using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for constraint-type knowledge entries.")]
    public class ConstraintContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for constraint content.")]
        [JsonPropertyName("type")]
        public override string Type => "constraint";

        [Description("Rule that defines the constraint.")]
        [JsonPropertyName("rule")]
        public string Rule { get; set; }

        [Description("Components that are affected by this constraint.")]
        [JsonPropertyName("affected_components")]
        public List<string> AffectedComponents { get; set; }

        [Description("Enforcement level for the constraint, such as hard or soft.")]
        [JsonPropertyName("enforcement")]
        public string Enforcement { get; set; }

        [Description("Effect or consequence if the constraint is violated.")]
        [JsonPropertyName("violation_effect")]
        public string ViolationEffect { get; set; }

        [Description("Optional metric describing quantitative aspects of the constraint.")]
        [JsonPropertyName("metric")]
        public ConstraintMetric Metric { get; set; }
    }
}

