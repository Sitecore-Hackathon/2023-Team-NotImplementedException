using Nest;

namespace GlitterBucket.Receive.Models
{
    [ElasticsearchType]
    public class IndexChangeModel
    {
        public DateTime Timestamp { get; init; }
        public Guid ItemId { get; init; }
        public Guid ParentId { get; init; }
        public int Version { get; set; }
        public Guid[] FieldIds { get; init; }
        public string Raw { get; init; }
        public string User { get; init; }
        public string SitecoreInstance { get; init; }
    }
}
