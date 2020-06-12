using System;
using Discord;
using questioneer.Core.Entities.Events;

namespace questioneer.Core.Entities
{
    public class ConfigFile : YamlFile
    {
        private const int NewestVersion = 0;

        protected override string ResourceName => "config.yml";

        public int Version { get; set; }
        public LogSeverity LogSeverity { get; set; }
        public string BotToken { get; set; }

        public event VersionMismatchHandler VersionMismatch;

        protected override void OnChanged()
        {
            Version = GetVersion();
            LogSeverity = GetLogSeverity();
            BotToken = GetBotToken();

            base.OnChanged();
        }

        private int GetVersion()
        {
            var versionValue = this["version"];
            int.TryParse(versionValue, out var version);

            if (version != NewestVersion)
                VersionMismatch?.Invoke(version, NewestVersion);

            return version;
        }

        public LogSeverity GetLogSeverity()
        {
            var loggingValue = this["logging"];
            Enum.TryParse(typeof(LogSeverity), loggingValue, true, out var logSeverity);
            return (LogSeverity)logSeverity;
        }

        public string GetBotToken()
        {
            return this["discord:token"];
        }
    }
}