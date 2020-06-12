using System;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using questioneer.Core.Entities;

namespace questioneer.Core.Services
{
    public class QuestionService
    {
        private readonly IServiceProvider _serviceProvider;

        private Timer _questionTimer;

        public QuestionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartQuestionAsync(Question question)
        {
            return SendQuestionAsync(question);
        }

        private async Task SendQuestionAsync(Question question)
        {
            var configurationService = _serviceProvider.GetRequiredService<ConfigurationService>();
            var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();

            var delay = configurationService.ConfigFile.Delay;
            var teams = configurationService.TeamsFile.Teams;
            var messages = configurationService.MessagesFile;

            var delayTimespan = TimeSpan.FromSeconds(delay);
            await Task.Delay(delayTimespan).ConfigureAwait(false);

            foreach (var team in teams)
            {
                var channel = discordClient.GetChannel(team.Channel);
                if (channel is SocketTextChannel textChannel)
                {
                    await textChannel.SendMessageAsync(messages.QuestionStarted).ConfigureAwait(false);
                }
            }
        }

        public void StopQuestion()
        {
        }
    }
}