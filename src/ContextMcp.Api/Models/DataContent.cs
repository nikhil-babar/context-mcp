using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for data-type knowledge entries.")]
    public class DataContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for data content.")]
        [JsonPropertyName("type")]
        public override string Type => "data";

        [Description("Name of the data entity being described.")]
        [JsonPropertyName("entity")]
        public string Entity { get; set; }

        [Description("Attributes that describe the entity and its properties.")]
        [JsonPropertyName("attributes")]
        public Dictionary<string, object> Attributes { get; set; }

        [Description("Source system from which the data originates.")]
        [JsonPropertyName("source_system")]
        public string SourceSystem { get; set; }

        [Description("Timestamp from which the data is considered valid.")]
        [JsonPropertyName("valid_from")]
        public DateTimeOffset? ValidFrom { get; set; }
    }
}

