using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace questioneer.Core.Entities
{
    public class ChannelsFile : YamlFile
    {
        protected override int NewestVersion => 0;
        protected override string ResourceName => "channels.yml";

        public ulong AnswersChannel { get; private set; }
        public List<Team> Teams { get; private set; }

        protected override void OnChanged()
        {
            base.OnChanged();

            AnswersChannel = GetAnswersChannel();
            Teams = GetTeams();
        }

        private ulong GetAnswersChannel()
        {
            var answersChannelValue = Configuration["answers_channel"];
            return ulong.Parse(answersChannelValue);
        }

        private List<Team> GetTeams()
        {
            var teams = new List<Team>();
            var teamsSection = Configuration.GetSection("teams");

            foreach (IConfigurationSection section in teamsSection.GetChildren())
            {
                var name = section["name"];
                var channelString = section["channel"];
                var channel = ulong.Parse(channelString);

                teams.Add(new Team { Name = name, Channel = channel });
            }

            return teams;
        }
    }
}