using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Metric describing quantitative limits for a constraint.")]
    public class ConstraintMetric
    {
        [Description("Name of the metric being measured.")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Description("Optional maximum allowed value for the metric.")]
        [JsonPropertyName("max_value")]
        public double? MaxValue { get; set; }

        [Description("Unit of measurement for the metric value.")]
        [JsonPropertyName("unit")]
        public string Unit { get; set; }
    }
}

