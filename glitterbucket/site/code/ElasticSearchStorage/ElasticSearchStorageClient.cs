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

        private static readonly Guid FieldIdEditor = new Guid("badd9cf9-53e0-4d0c-bcc0-2d784c282f6a");

        private string IndexName => $"{IndexPrefix}-{DateTime.UtcNow:yyyy.MM}";

        public async Task Add(string sitecoreInstanceId, SitecoreWebHookModel model, string? raw = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var now = DateTime.UtcNow;

            if (raw == null)
            {
                using var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, model);
                using var reader = new StreamReader(stream);
                raw = await reader.ReadToEndAsync();
            }

            var indexName = IndexName;

            var fieldIds = model.Changes?.FieldChanges?.Select(x => x.FieldId).ToArray() ?? Array.Empty<Guid>();
            var userName = model.Changes?.FieldChanges?.FirstOrDefault(x => x.FieldId == FieldIdEditor)?.Value;
            var fields = new IndexChangeModel
            {
                Timestamp = now,
                EventName = model.EventName,
                Raw = raw,
                ItemId = model.Item?.Id ?? Guid.Empty,
                Version = model.Item?.Version ?? 0,
                ParentId = model.Item?.ParentId ?? Guid.Empty,
                Language = model.Item?.Language,
                SitecoreInstance = sitecoreInstanceId,
                FieldIds = fieldIds,
                User = userName,
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

        public async Task<IEnumerable<IndexChangeModel>> GetByItem(Guid itemId, string language, int? version)
        {
            var result = await _client.SearchAsync<IndexChangeModel>(s =>
                s
                    .Index(IndexPrefix + "*")
                    .Query(query =>  query.Bool(b => b.Filter(
                            q => q.Term(f => f.ItemId, itemId),
                            q => q.Term(f => f.Language, language),
                            q => q.Term(f => f.Version, version)
                            )))
                    .Fields(fl => fl.Fields(f => f.Timestamp, f => f.User, f => f.FieldIds))
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
