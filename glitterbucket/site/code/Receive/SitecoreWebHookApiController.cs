using System.Text;
using System.Text.Json;
using GlitterBucket.Receive.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace GlitterBucket.Receive
{
    [ApiController]
    public class SitecoreWebHookApiController : Controller
    {
        private readonly ElasticClient _client;

        public SitecoreWebHookApiController(ElasticClient client)
        {
            _client = client;
        }

        [HttpPost("hook/{id}")]
        public async Task<IActionResult> Receive(string id)
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var raw = await reader.ReadToEndAsync();
            var now = DateTime.UtcNow;

            await using (var stdOut = Console.OpenStandardOutput())
            {
                await JsonSerializer.SerializeAsync(stdOut, raw);
            }

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(raw));
            var model = await JsonSerializer.DeserializeAsync<SitecoreWebHookModel>(stream);

            var indexName = $"glitteraudit-{now:yyyy-MM}";
            await _client.Indices.CreateAsync(indexName, s => s
                .Settings(se => se
                .NumberOfReplicas(1)
            ).Map(m => m.AutoMap()));

            var fieldIds = model?.Changes?.FieldChanges?.Select(x => x.FieldId).ToArray() ?? Array.Empty<Guid>();
            var fields = new IndexChangeModel
            {
                Timestamp = now,
                Raw = raw,
                ItemId = model?.Item?.Id ?? Guid.Empty,
                Version = model?.Item?.Version ?? 0,
                ParentId = model?.Item?.ParentId ?? Guid.Empty,
                SitecoreInstance = id,
                FieldIds = fieldIds,
            };
            await _client.CreateAsync(fields, opt => opt.Index(indexName).Id(Guid.NewGuid()));

            return Ok();
        }
    }
}
