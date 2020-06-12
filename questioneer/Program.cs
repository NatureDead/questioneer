using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using questioneer.Core.Entities;
using questioneer.Core.Services;

namespace questioneer
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        internal static async Task Main(string[] args)
        {
            Reflections.AppDomain.ProcessExit += ProcessOnExited;

            try
            {
                _serviceProvider = ServiceBuilder.Build();

                var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
                var commandService = _serviceProvider.GetRequiredService<CommandService>();
                var configurationSerivce = _serviceProvider.GetRequiredService<ConfigurationService>();

                discordClient.Log += LogAsync;
                commandService.Log += LogAsync;

                await discordClient.LoginAsync(TokenType.Bot, configurationSerivce.ConfigFile.BotToken).ConfigureAwait(false);
                await discordClient.StartAsync().ConfigureAwait(false);

                discordClient.MessageReceived += async (socketMessage) =>
                {
                    if (!(socketMessage is SocketUserMessage socketUserMessage)) return;
                    if (socketUserMessage.Source != MessageSource.User) return;

                    var argPos = 1;
                    if (!socketUserMessage.HasMentionPrefix(discordClient.CurrentUser, ref argPos))
                        return;

                    var context = new SocketCommandContext(discordClient, socketUserMessage);
                    await commandService.ExecuteAsync(context, argPos, _serviceProvider).ConfigureAwait(false);
                };

                await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            await Task.Delay(-1).ConfigureAwait(false);
        }

        private static void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            if (_serviceProvider == null) return;

            var discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();

            discordClient.StopAsync().GetAwaiter().GetResult();
            discordClient.LogoutAsync().GetAwaiter().GetResult();

            _serviceProvider.Dispose();
        }

        private static Task LogAsync(LogMessage logMessage)
        {
            LogMessage(logMessage.Severity, logMessage.Message);
            return Task.CompletedTask;
        }

        private static void LogMessage(LogSeverity logSeverity, string message)
        {
            Console.WriteLine($"[{DateTime.Now:T}] [{logSeverity}] {message}");
        }
    }
}