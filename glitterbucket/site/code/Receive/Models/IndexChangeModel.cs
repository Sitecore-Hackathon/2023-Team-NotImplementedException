using Nest;

namespace GlitterBucket.Receive.Models
{
    [ElasticsearchType]
    public class IndexChangeModel
    {
        public DateTime Timestamp { get; init; }
        public Guid ItemId { get; init; }
        public Guid ParentId { get; init; }
        public Guid[] FieldIds { get; init; }
        public SitecoreWebHookModel Raw { get; init; }
        public string SitecoreInstance { get; init; }
    }
}
