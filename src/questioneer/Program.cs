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
                var configurationService = _serviceProvider.GetRequiredService<ConfigurationService>();

                #region startup

                discordClient.Log += LogAsync;
                commandService.Log += LogAsync;

                await discordClient.LoginAsync(TokenType.Bot, configurationService.ConfigFile.BotToken).ConfigureAwait(false);
                await discordClient.StartAsync().ConfigureAwait(false);

                #endregion startup

                #region command handler

                discordClient.MessageReceived += async (message) =>
                {
                    if (!(message is SocketUserMessage userMessage)) return;
                    if (userMessage.Source != MessageSource.User) return;

                    var argPos = 1;
                    if (userMessage.HasMentionPrefix(discordClient.CurrentUser, ref argPos) ||
                        userMessage.HasCharPrefix('!', ref argPos))
                    {
                        var commandContext = new SocketCommandContext(discordClient, userMessage);
                        await commandService.ExecuteAsync(commandContext, argPos, _serviceProvider).ConfigureAwait(false);
                    }
                };

                await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider).ConfigureAwait(false);

                #endregion command handler
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