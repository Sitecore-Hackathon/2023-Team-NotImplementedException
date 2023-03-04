using System.Text.Json;
using GlitterBucket.Shared;
using Nest;

namespace GlitterBucket.ElasticSearchStorage
{
    public class ElasticSearchStorageClient : IStorageClient
    {
        private readonly ElasticClient _client;

        public ElasticSearchStorageClient(ElasticClient client)
        {
            _client = client;
        }

        private const string IndexPrefix = "glitteraudit";

        private string IndexName => $"{IndexPrefix}-{DateTime.UtcNow:yyyy-MM}";

        public async Task Add(string sitecoreInstanceId, SitecoreWebHookModel model, string? raw = null)
        {
            var now = DateTime.UtcNow;

            if (raw == null)
            {
                using var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, model);
                using var reader = new StreamReader(stream);
                raw = await reader.ReadToEndAsync();
            }

            var indexName = IndexName;

            var fieldIds = model?.Changes?.FieldChanges?.Select(x => x.FieldId).ToArray() ?? Array.Empty<Guid>();
            var fields = new IndexChangeModel
            {
                Timestamp = now,
                EventName = model.EventName,
                Raw = raw,
                ItemId = model.Item?.Id ?? Guid.Empty,
                Version = model.Item?.Version ?? 0,
                ParentId = model.Item?.ParentId ?? Guid.Empty,
                SitecoreInstance = sitecoreInstanceId,
                FieldIds = fieldIds,
            };
            await _client.CreateAsync(fields, opt => opt.Index(indexName).Id(Guid.NewGuid()));
        }

        public async Task<IEnumerable<IndexChangeModel>> GetByItemId(Guid itemId)
        {
            var result = await _client.SearchAsync<IndexChangeModel>(s =>
                s
                    .Index(IndexPrefix + "*")
                    .Query(q => q.Term(f => f.ItemId, itemId))
                    .Fields(fl => fl.Fields(f => f.Raw))
                    .Sort(o => o.Descending(f => f.Timestamp))
                );
            return result.Hits.Select(hit => hit.Source);
        }

        private Task EnsureIndex(string indexName)
        {
            return _client.Indices.CreateAsync(indexName, s => s
                .Settings(se => se
                    .NumberOfReplicas(1)
                ).Map(m => m.AutoMap()));

        }
    }
}
