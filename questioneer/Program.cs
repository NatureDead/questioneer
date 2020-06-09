using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using questioneer.Services;
using System;
using System.Threading.Tasks;

namespace questioneer
{
    internal class Program
    {
        private static DiscordSocketClient _discordClient;
        private static CommandService _commandService;
        private static IConfiguration _configuration;

        internal static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("config.yml");

            _configuration = configurationBuilder.Build();

            using (var services = ConfigureServices())
            {
                // get the client and assign to client 
                // you get the services via GetRequiredService<T>
                _discordClient = services.GetRequiredService<DiscordSocketClient>();
                _commandService = services.GetRequiredService<CommandService>();

                // setup logging and the ready event
                _discordClient.Log += LogAsync;
                _commandService.Log += LogAsync;

                // this is where we get the Token value from the configuration file, and start the bot
                await _discordClient.LoginAsync(TokenType.Bot, _configuration["token"]);
                await _discordClient.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }
        }

        private static Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }
    }
}
