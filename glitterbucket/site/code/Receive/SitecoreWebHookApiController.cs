using System.Text;
using System.Text.Json;
using GlitterBucket.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GlitterBucket.Receive
{
    [ApiController]
    public class SitecoreWebHookApiController : Controller
    {
        private readonly IStorageClient _client;
        private readonly ILogger<SitecoreWebHookApiController> _logger;

        public SitecoreWebHookApiController(IStorageClient client, ILogger<SitecoreWebHookApiController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpPost("hook/{id}")]
        public async Task<IActionResult> Receive(string id)
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var raw = await reader.ReadToEndAsync();
            _logger.LogInformation("Received from {SitecoreId}", id);

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(raw));
            var model = await JsonSerializer.DeserializeAsync<SitecoreWebHookModel>(stream);
            _logger.LogInformation("Received {EventName} from {SitecoreId}: {Raw}", model?.EventName, id, raw);

            await _client.Add(id, model, raw);

            return Ok();
        }
    }
}
