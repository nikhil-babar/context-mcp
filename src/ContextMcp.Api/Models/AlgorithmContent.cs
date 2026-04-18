using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for algorithm-type knowledge entries.")]
    public class AlgorithmContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for algorithm content.")]
        [JsonPropertyName("type")]
        public override string Type => "algorithm";

        [Description("Statement describing the problem the algorithm solves.")]
        [JsonPropertyName("problem_statement")]
        public string ProblemStatement { get; set; }

        [Description("Inputs expected by the algorithm.")]
        [JsonPropertyName("inputs")]
        public List<string> Inputs { get; set; }

        [Description("Outputs produced by the algorithm.")]
        [JsonPropertyName("outputs")]
        public List<string> Outputs { get; set; }

        [Description("Ordered steps that define the algorithm.")]
        [JsonPropertyName("steps")]
        public List<string> Steps { get; set; }

        [Description("Time complexity description of the algorithm.")]
        [JsonPropertyName("time_complexity")]
        public string TimeComplexity { get; set; }

        [Description("Space complexity description of the algorithm.")]
        [JsonPropertyName("space_complexity")]
        public string SpaceComplexity { get; set; }
    }
}

