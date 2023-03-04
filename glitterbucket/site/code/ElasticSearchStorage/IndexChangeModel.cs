using Nest;

namespace GlitterBucket.ElasticSearchStorage
{
    [ElasticsearchType]
    public class IndexChangeModel
    {
        public DateTime Timestamp { get; init; }
        [Keyword]
        public string EventName { get; init; }

        [Keyword]
        public Guid ItemId { get; init; }
        public Guid ParentId { get; init; }
        [Keyword]
        public int Version { get; set; }
        public Guid[] FieldIds { get; init; }
        public string Raw { get; init; }
        [Keyword]
        public string User { get; init; }
        [Keyword]
        public string SitecoreInstance { get; init; }
    }
}
