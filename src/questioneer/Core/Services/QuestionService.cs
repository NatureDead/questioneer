using System;
using System.Threading.Tasks;
using System.Timers;
using questioneer.Core.Entities;

namespace questioneer.Core.Services
{
    public class QuestionService
    {
        private readonly ILogService _logService;
        private readonly ConfigurationService _configurationService;
        private readonly DiscordService _discordService;
        private readonly AnswerService _answerService;

        private Timer _questionTimer;

        public QuestionService(ILogService logService, ConfigurationService configurationService,
            DiscordService discordService, AnswerService answerService)
        {
            _logService = logService;
            _configurationService = configurationService;
            _discordService = discordService;
            _answerService = answerService;
        }

        public async Task StartQuestionAsync(Question question)
        {
            try
            {
                var delayTimespan = _configurationService.ConfigFile.Delay;
                await Task.Delay(delayTimespan).ConfigureAwait(false);

                var durationTimespan = TimeSpan.FromSeconds(question.Duration);
                _questionTimer = new Timer(durationTimespan.TotalMilliseconds);
                _questionTimer.AutoReset = false;
                _questionTimer.Elapsed += async (sender, eventArgs) => await StopQuestionAsync(question).ConfigureAwait(false);
                _questionTimer.Start();

                var answersMessage = default(AnswersMessage);
                await _answerService.SendAnswersMessageAsync(answersMessage).ConfigureAwait(false);

                var messages = _configurationService.MessagesFile;
                var questionStartedMessage = messages.QuestionStarted(question.Name);

                await SendMessageToTeamsAsync(questionStartedMessage).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logService.Log(exception);
            }
        }

        public async Task StopQuestionAsync(Question question)
        {
            try
            {
                var messages = _configurationService.MessagesFile;
                var questionStoppedMessage = messages.QuestionStopped(question.Name);

                await SendMessageToTeamsAsync(questionStoppedMessage).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logService.Log(exception);
            }
        }

        private async Task SendMessageToTeamsAsync(string message)
        {
            var teams = _configurationService.ChannelsFile.Teams;
            foreach (var team in teams)
                await _discordService.SendMessageAsync(team.Channel, message).ConfigureAwait(false);
        }
    }
}