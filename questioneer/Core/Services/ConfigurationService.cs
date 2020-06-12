﻿using questioneer.Core.Entities;

namespace questioneer.Core.Services
{
    public class ConfigurationService
    {
        public ConfigFile ConfigFile { get; }
        public TeamsFile TeamsFile { get; }

        public ConfigurationService()
        {
            ConfigFile = YamlFile.Get<ConfigFile>();
            TeamsFile = YamlFile.Get<TeamsFile>();
        }
    }
}