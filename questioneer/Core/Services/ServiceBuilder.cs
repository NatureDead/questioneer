using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace questioneer.Core.Services
{
    public static class ServiceBuilder
    {
        public static ServiceProvider Build()
        {
            var configurationService = new ConfigurationService();

            return new ServiceCollection()
                .AddSingleton(configurationService)
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = configurationService.ConfigFile.LogSeverity
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = configurationService.ConfigFile.LogSeverity
                }))
                .BuildServiceProvider();
        }
    }
}