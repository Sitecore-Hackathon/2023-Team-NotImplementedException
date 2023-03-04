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
        public async Task<IActionResult> Receive(string id, [FromBody]SitecoreWebHookModel model)
        {
            using var stdOut = Console.OpenStandardOutput();
            await JsonSerializer.SerializeAsync(stdOut, model);

            return Ok();
        }
    }
}
