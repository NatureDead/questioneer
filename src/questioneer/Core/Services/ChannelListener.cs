using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace questioneer.Core.Services
{
    public class ChannelListener : IDisposable
    {
        private readonly List<ulong> _channelIds;
        private readonly DiscordSocketClient _discordSocketClient;

        public ChannelListener(DiscordSocketClient discordSocketClient, IEnumerable<ulong> channelIds)
        {
            if (discordSocketClient == null)
                throw new ArgumentNullException(nameof(discordSocketClient));

            if (channelIds == null)
                throw new ArgumentNullException(nameof(channelIds));

            _discordSocketClient = discordSocketClient;
            _channelIds = new List<ulong>(channelIds);

            Start();
        }

        public void Dispose()
        {
            Stop();
        }

        private void Start()
        {
            _discordSocketClient.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            if (!(message is SocketUserMessage userMessage)) return;
            if (userMessage.Source != MessageSource.User) return;

            if (_channelIds.Contains(userMessage.Channel.Id))
            {

            }
        }

        private void Stop()
        {
            _discordSocketClient.MessageReceived -= OnMessageReceived;
        }
    }
}