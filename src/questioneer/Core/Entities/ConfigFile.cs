using System;
using Discord;
using questioneer.Core.Entities.Events;

namespace questioneer.Core.Entities
{
    public class ConfigFile : YamlFile
    {
        private const int NewestVersion = 0;

        protected override string ResourceName => "config.yml";

        public int Version { get; private set; }
        public LogSeverity LogSeverity { get; private set; }
        public string BotToken { get; private set; }
        public byte Delay { get; private set; }

        public event VersionMismatchHandler VersionMismatch;

        protected override void OnChanged()
        {
            Version = GetVersion();
            LogSeverity = GetLogSeverity();
            BotToken = GetBotToken();
            Delay = GetDelay();

            base.OnChanged();
        }

        private int GetVersion()
        {
            var versionValue = Configuration["version"];
            var version = int.Parse(versionValue);

            if (version != NewestVersion)
                VersionMismatch?.Invoke(version, NewestVersion);

            return version;
        }

        public LogSeverity GetLogSeverity()
        {
            var loggingValue = Configuration["logging"];
            return (LogSeverity)Enum.Parse(typeof(LogSeverity), loggingValue, true);
        }

        public string GetBotToken()
        {
            return Configuration["discord:token"];
        }

        private byte GetDelay()
        {
            var delayValue = Configuration["delay"];
            return byte.Parse(delayValue);
        }
    }
}