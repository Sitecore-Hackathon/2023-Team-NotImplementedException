using GlitterBucket.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GlitterBucket.BrowserExtension
{
    [ApiController]
    public class BrowserExtensionRetrieveController : Controller
    {
        private readonly IStorageClient _client;

        public BrowserExtensionRetrieveController(IStorageClient client)
        {
            _client = client;
        }

        [Route("item/{itemId}")]
        public async Task<IActionResult> GetByItem(Guid itemId)
        {
            var result = _client.GetByItemId(itemId);

            return Ok(result);
        }
    }
}
