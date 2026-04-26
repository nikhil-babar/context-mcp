using ContextMcp.Api.Config;
using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Services;
using ContextMcp.Api.Services.VoyageService;
using Microsoft.Extensions.Options;

namespace ContextMcp.Api.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ElasticSearchOptions>(
                 config.GetSection(ElasticSearchOptions.Section));

            var options = config
                .GetSection(ElasticSearchOptions.Section)
                .Get<ElasticSearchOptions>()!;

            services.AddSingleton(ElasticsearchClientFactory.Create(options));

            return services;
        }

        public static IServiceCollection AddVoyageEmbeddingService(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<VoyageEmbeddingOptions>(
                config.GetSection(VoyageEmbeddingOptions.Section));

            var voyageOptions = config
                .GetSection(VoyageEmbeddingOptions.Section)
                .Get<VoyageEmbeddingOptions>()!;

            services.AddHttpClient<IEmbeddingService, VoyageEmbeddingService>(client =>
            {
                var baseUri = new Uri(voyageOptions.BaseUrl);
                client.BaseAddress = new Uri(baseUri.GetLeftPart(UriPartial.Authority));
                client.Timeout = voyageOptions.Timeout;

                if (!string.IsNullOrEmpty(voyageOptions.ApiKey))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", voyageOptions.ApiKey);
                }
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = 100
            });

            return services;
        }

        public static IServiceCollection AddServices(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IKnowledgeBaseService, KnowledgeBaseService>();

            return services;
        }
    }
}
