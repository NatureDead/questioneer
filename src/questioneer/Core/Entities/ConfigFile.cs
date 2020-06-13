using System;
using Discord;

namespace questioneer.Core.Entities
{
    public class ConfigFile : YamlFile
    {
        protected override int NewestVersion => 0;
        protected override string ResourceName => "config.yml";

        public LogSeverity LogSeverity { get; private set; }
        public string BotToken { get; private set; }
        public byte Delay { get; private set; }

        protected override void OnChanged()
        {
            base.OnChanged();

            LogSeverity = GetLogSeverity();
            BotToken = GetBotToken();
            Delay = GetDelay();
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