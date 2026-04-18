// Infrastructure/Elasticsearch/ElasticsearchClientFactory.cs
using ContextMcp.Api.Config;
using ContextMcp.Api.Utility;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Serialization;
using Elastic.Transport;

public static class ElasticsearchClientFactory
{
    public static ElasticsearchClient Create(ElasticSearchSettings options)
    {
        var nodePool = new SingleNodePool(new Uri(options.Url));

        var settings = new ElasticsearchClientSettings(
            nodePool,
            sourceSerializer: (defaultSerializer, settings) =>
                new DefaultSourceSerializer(settings, options =>
                {
                    options.Converters.Add(new KnowledgeBaseContentJsonConverter());
                }));

        if (!string.IsNullOrEmpty(options.ApiKey))
            settings = settings.Authentication(new ApiKey(options.ApiKey));

        if (options.EnableDebugMode)
            settings = settings.EnableDebugMode().PrettyJson();

        return new ElasticsearchClient(settings);
    }
}