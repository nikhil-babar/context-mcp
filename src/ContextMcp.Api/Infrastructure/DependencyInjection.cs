using ContextMcp.Api.Config;
using ContextMcp.Api.Interfaces;
using ContextMcp.Api.Services;
using System.Text.Json;

namespace ContextMcp.Api.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration config)
        {
            var options = config
                .GetSection("Elasticsearch")
                .Get<ElasticSearchSettings>()!;

            services.AddSingleton(ElasticsearchClientFactory.Create(options));

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
