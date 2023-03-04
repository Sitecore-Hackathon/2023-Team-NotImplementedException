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

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetByItem(Guid itemId)
        {
            var result = await _client.GetByItemId(itemId);

            return Ok(result);
        }

        [HttpGet("item/{itemId}/version/{version}")]
        public async Task<IActionResult> GetByItem(Guid itemId, int version)
        {
            var result = await _client.GetByItemId(itemId);

            var mock = new[]
            {
                new ExtensionChangeModel
                {
                    Timestamp = new DateTime(2022, 1, 1, 1, 1, 0),
                    Username = @"sitecore\cni",
                    FieldsText = "bla bla",
                },
                new ExtensionChangeModel
                {
                    Timestamp = DateTime.UtcNow,
                    Username = @"sitecore\jba",
                    FieldsText = "bla bla2",
                }
            };

            return Ok(mock);
        }

    }
}
