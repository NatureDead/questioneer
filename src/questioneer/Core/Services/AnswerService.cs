using System;
using System.Threading.Tasks;
using questioneer.Core.Entities;

namespace questioneer.Core.Services
{
    public class AnswerService
    {
        private readonly ILogService _logService;
        private readonly ConfigurationService _configurationService;
        private readonly DiscordService _discordService;

        public AnswerService(ILogService logService, ConfigurationService configurationService,
            DiscordService discordService)
        {
            _logService = logService;
            _configurationService = configurationService;
            _discordService = discordService;
        }

        public async Task SendAnswersMessageAsync(Question question, AnswersMessage answersMessage)
        {
            try
            {
                var answersChannel = _configurationService.ChannelsFile.AnswersChannel;
                var teams = _configurationService.ChannelsFile.Teams;

                // Build answers message
                await _discordService.SendMessageAsync(answersChannel, "Some").ConfigureAwait(false);

                foreach (var team in teams)
                {
                    answersMessage.AnswersByTeams.TryGetValue(team, out var answerMessage);


                    await _discordService.SendMessageAsync(answersChannel, ).ConfigureAwait(false);
                }

                await _discordService.SendMessageAsync(answersChannel, "You").ConfigureAwait(false);
                await _discordService.SendMessageAsync(answersChannel, "See").ConfigureAwait(false);
                await _discordService.SendMessageAsync(answersChannel, "Here").ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logService.Log(exception);
            }
        }
    }
}