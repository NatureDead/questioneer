using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace questioneer.Core.Services
{
    public class DiscordService
    {
        public DiscordSocketClient DiscordSocketClient { get; }

        public DiscordService(DiscordSocketClient discordSocketClient)
        {
            DiscordSocketClient = discordSocketClient;
        }

        public Task<IUserMessage> SendMessageAsync(ulong channelId, string message)
        {
            var channel = DiscordSocketClient.GetChannel(channelId);
            if (channel is IMessageChannel messageChannel)
                return messageChannel.SendMessageAsync(message);

            return Task.FromResult(default(IUserMessage));
        }
    }
}