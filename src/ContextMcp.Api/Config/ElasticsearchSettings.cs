namespace ContextMcp.Api.Config
{
    public class ElasticSearchSettings
    {
        public string KnowledgeBaseIndexName { get; set; } = string.Empty;
        public string KnowledgeBaseRelationIndexName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string DefaultIndex { get; set; } = string.Empty;
        public string? ApiKey { get; set; }
        public bool EnableDebugMode { get; set; } = false;
    }
}
