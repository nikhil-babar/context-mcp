namespace ContextMcp.Api.Config
{
    public class ElasticSearchOptions
    {
        public static readonly string Section = "Elasticsearch";
        public string KnowledgeBaseIndexName { get; set; } = string.Empty;
        public string KnowledgeBaseRelationIndexName { get; set; } = string.Empty;
        public string UserIndexName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string DefaultIndex { get; set; } = string.Empty;
        public string? ApiKey { get; set; }
        public bool EnableDebugMode { get; set; } = false;
    }
}
