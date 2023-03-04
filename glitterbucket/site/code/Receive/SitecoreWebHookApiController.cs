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

        public SitecoreWebHookApiController(IStorageClient client)
        {
            _client = client;
        }

        [HttpPost("hook/{id}")]
        public async Task<IActionResult> Receive(string id)
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var raw = await reader.ReadToEndAsync();

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(raw));
            var model = await JsonSerializer.DeserializeAsync<SitecoreWebHookModel>(stream);

            await _client.Add(id, model, raw);

            return Ok();
        }
    }
}
