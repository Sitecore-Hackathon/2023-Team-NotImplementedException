using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;

namespace GlitterBucket.Receive
{
    [ApiController]
    public class SitecoreWebHookApiController : Controller
    {
        private readonly ILogger<SitecoreWebHookApiController> _logger;
        private readonly ChannelWriter<ReceivedWebhookData> _channel;

        public SitecoreWebHookApiController(ILogger<SitecoreWebHookApiController> logger, ChannelWriter<ReceivedWebhookData> channel)
        {
            _logger = logger;
            _channel = channel;
        }

        [HttpPost("hook/{id}")]
        public async Task<IActionResult> Receive(string id)
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var raw = await reader.ReadToEndAsync();
            _logger.LogInformation("Received from {SitecoreId}", id);

            await _channel.WriteAsync(new ReceivedWebhookData
            {
                SitecoreInstanceId = id,
                ReceivedData = raw
            });

            return Ok();
        }
    }
}
