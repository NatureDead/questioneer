using questioneer.Core.Entities;

namespace questioneer.Core.Services
{
    public class ConfigurationService
    {
        public ConfigFile ConfigFile { get; }

        public ConfigurationService()
        {
            ConfigFile = YamlFile.Get<ConfigFile>();
        }
    }
}