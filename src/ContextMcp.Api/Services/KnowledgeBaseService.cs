using ContextMcp.Api.Config;
using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Models.ToolRequests;
using ContextMcp.Api.Models.ToolResponses;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace ContextMcp.Api.Services
{
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly ElasticSearchSettings _elasticSearchSettings;

        public KnowledgeBaseService(
            ElasticsearchClient elasticsearchClient,
            IOptions<ElasticSearchSettings> elasticSearchSettingsOptions)
        {
            _elasticsearchClient = elasticsearchClient;
            _elasticSearchSettings = elasticSearchSettingsOptions.Value;
        }

        public async Task<PostKnowledgeBaseToolResponse> PostKnowledgeAsync(PostKnowledgeBaseToolRequest request, CancellationToken cancellationToken)
        {
            var indexName = _elasticSearchSettings.KnowledgeBaseIndexName;

            var response = await _elasticsearchClient.IndexAsync(request.KnowledgeBase, x => 
            {
                x.Index(indexName);
                if (!string.IsNullOrEmpty(request.KnowledgeBase.Id))
                {
                    x.Id(request.KnowledgeBase.Id);
                }
            }, cancellationToken);

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to post knowledge base. Error: {response.DebugInformation}");
            }

            return new PostKnowledgeBaseToolResponse
            {
                Id = response.Id
            };
        }
    }
}
