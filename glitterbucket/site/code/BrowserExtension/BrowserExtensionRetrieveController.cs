using GlitterBucket.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GlitterBucket.BrowserExtension
{
    [ApiController]
    [EnableCors(PolicyName = CorsPolicyName)]
    public class BrowserExtensionRetrieveController : Controller
    {
        public const string CorsPolicyName = "extension";

        private readonly IStorageClient _client;

        public BrowserExtensionRetrieveController(IStorageClient client)
        {
            _client = client;
        }

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetByItem(Guid itemId)
        {
            var result = await _client.GetByItemId(itemId);

            return Ok(result);
        }

        [HttpGet("item/{itemId}/language/{language}/version/{version}")]
        public async Task<IActionResult> GetByItem(Guid itemId, string language, int version)
        {
            var result = await _client.GetByItemId(itemId);

            var data = result.Select(x => new ExtensionChangeModel
            {
                Timestamp = x.Timestamp,
                Username = x.User,
            }).ToArray();
            return Ok(data);
        }

    }
}
