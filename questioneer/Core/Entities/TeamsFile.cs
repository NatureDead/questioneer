using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace questioneer.Core.Entities
{
    public class TeamsFile : YamlFile
    {
        protected override string ResourceName => "teams.yml";

        public List<Team> Teams { get; private set; }

        protected override void OnChanged()
        {
            Teams = GetTeams();

            base.OnChanged();
        }

        private List<Team> GetTeams()
        {
            var teams = new List<Team>();

            var teamsSection = Configuration.GetSection("teams");
            foreach (IConfigurationSection section in teamsSection.GetChildren())
            {
                var name = section["name"];
                var channelString = section["channel"];
                var channel = long.Parse(channelString);

                teams.Add(new Team { Name = name, Channel = channel });
            }

            return teams;
        }
    }
}