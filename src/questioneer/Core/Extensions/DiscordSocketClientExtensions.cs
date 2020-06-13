using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using questioneer.Core.Entities;

namespace questioneer.Core.Extensions
{
    public static class DiscordSocketClientExtensions
    {
        public static async Task SendMessageAsync(this DiscordSocketClient discordClient, IEnumerable<Team> teams, string message)
        {
            foreach (var team in teams)
            {
                var channel = discordClient.GetChannel(team.Channel);
                if (channel is IMessageChannel messageChannel)
                    await messageChannel.SendMessageAsync(message).ConfigureAwait(false);
            }
        }
    }
}