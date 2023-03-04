using GlitterBucket.ElasticSearchStorage;

namespace GlitterBucket.Shared
{
    public interface IStorageClient
    {
        Task Add(string sitecoreInstanceId, SitecoreWebHookModel model, string? raw = null);

        Task<IEnumerable<IndexChangeModel>> GetByItemId(Guid itemId);
    }
}
