using System.Collections.Generic;
using Discord;

namespace questioneer.Core.Entities
{
    public class AnswersMessage
    {
        public IUserMessage Header { get; set; }
        public Dictionary<Team, IUserMessage> AnswersByTeams { get; set; }
    }
}