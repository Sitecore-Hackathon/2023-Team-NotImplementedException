using System.Threading.Channels;

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
        }
    }
}
