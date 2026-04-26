using ContextMcp.Api.Config;
using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Models.Auth;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Options;

namespace ContextMcp.Api.Stores
{
    public class UserStore : IUserStore
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly ElasticSearchOptions _elasticSearchSettings;

        public UserStore(
            ElasticsearchClient elasticsearchClient,
            IOptions<ElasticSearchOptions> elasticSearchSettingsOptions)
        {
            _elasticsearchClient = elasticsearchClient;
            _elasticSearchSettings = elasticSearchSettingsOptions.Value;
        }

        public async Task<User?> GetUserByApiKeyHashAsync(string apiKeyHash)
        {
            var indexName = _elasticSearchSettings.UserIndexName;

            var response = await _elasticsearchClient.SearchAsync<User>(s => s
                .Indices(indexName)
                .Query(q => q
                    .Nested(n => n
                        .Path(p => p.ApiKeys)
                        .Query(nq => nq
                            .Term(t => t.Field("api_keys.key").Value(apiKeyHash))
                        )
                    )
                )
            );

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to search user by API key. {response.DebugInformation}");
            }

            return response.Documents.FirstOrDefault();
        }
    }
}
