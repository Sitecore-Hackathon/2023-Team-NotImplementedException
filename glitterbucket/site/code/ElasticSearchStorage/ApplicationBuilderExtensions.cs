using Elasticsearch.Net;
using GlitterBucket.Shared;
using Nest;

namespace GlitterBucket.ElasticSearchStorage
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddElasticSearchStorage(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient(sp => sp.GetRequiredService<IConfiguration>().GetSection("ElasticSearch").Get<ElasticSearchStorage>());
            builder.Services.AddTransient<IConnectionPool>(sp => new SingleNodeConnectionPool(sp.GetRequiredService<ElasticSearchStorage>().Uri));
            builder.Services.AddTransient(sp => new ElasticClient(new ConnectionSettings(sp.GetRequiredService<IConnectionPool>())));

            builder.Services.AddTransient<IStorageClient, ElasticSearchStorageClient>();
        }
    }
}
