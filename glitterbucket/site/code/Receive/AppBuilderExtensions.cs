using System.Threading.Channels;
using GlitterBucket.Receive.Configuration;

namespace GlitterBucket.Receive
{
    public static class AppBuilderExtensions
    {
        public static void AddReceiveServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<QueuedHostedService>();

            builder.Services.AddSingleton(_ => Channel.CreateUnbounded<ReceivedWebhookData>());
            builder.Services.AddSingleton(sp => sp.GetRequiredService<Channel<ReceivedWebhookData>>().Reader);
            builder.Services.AddSingleton(sp => sp.GetRequiredService<Channel<ReceivedWebhookData>>().Writer);

            builder.Services.AddTransient<ReceivingInstance>(sp => sp.GetRequiredService<IConfiguration>().GetSection("Sitecore").Get<ReceivingInstance>() ?? new ReceivingInstance());
        }
    }
}
