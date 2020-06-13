using System;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using questioneer.Core.Entities;
using questioneer.Core.Extensions;

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

        public async Task StartQuestionAsync(Question question)
        {
            var configurationService = _serviceProvider.GetRequiredService<ConfigurationService>();

            var delay = configurationService.ConfigFile.Delay;
            var teams = configurationService.TeamsFile.Teams;
            var messages = configurationService.MessagesFile;

            var delayTimespan = TimeSpan.FromSeconds(delay);
            await Task.Delay(delayTimespan).ConfigureAwait(false);

            var durationTimespan = TimeSpan.FromSeconds(question.Duration);
            _questionTimer = new Timer(durationTimespan.TotalMilliseconds);
            _questionTimer.AutoReset = false;
            _questionTimer.Elapsed += async (sender, eventArgs) => await StopQuestionAsync(question).ConfigureAwait(false);
            _questionTimer.Start();

            var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            var questionStartedMessage = messages.QuestionStarted(question.Name);

            await discordClient.SendMessageAsync(teams, questionStartedMessage).ConfigureAwait(false);
        }

        public async Task StopQuestionAsync(Question question)
        {
            var configurationService = _serviceProvider.GetRequiredService<ConfigurationService>();

            var teams = configurationService.TeamsFile.Teams;
            var messages = configurationService.MessagesFile;

            var questionStoppedMessage = messages.QuestionStopped(question.Name);

            var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
            await discordClient.SendMessageAsync(teams, questionStoppedMessage);
        }
    }
}