using questioneer.Core.Entities;

namespace questioneer.Core.Services
{
    public class ConfigurationService
    {
        public ConfigFile ConfigFile { get; }
        public ChannelsFile ChannelsFile { get; }
        public MessagesFile MessagesFile { get; }

        public ConfigurationService()
        {
            ConfigFile = YamlFile.Get<ConfigFile>();
            ChannelsFile = YamlFile.Get<ChannelsFile>();
            MessagesFile = YamlFile.Get<MessagesFile>();
        }
    }
}