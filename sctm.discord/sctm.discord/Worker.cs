using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace sctm.discord
{
    public class Worker : BackgroundService
    {
        private IConfiguration _config;
        private string _assemblyVersion;
        public readonly EventId BotEventId = new EventId(42, "ChrispyKangaroo");

        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; set; }

        public Worker(IConfiguration config)
        {
            _config = config;

            var _sourceName = _config["Logging:SourceName"];
            var _logName = _config["Logging:LogName"];

            Log.Logger = new LoggerConfiguration()
                .WriteTo.EventLog(_sourceName, _logName, manageEventSource: false)
                .WriteTo.Console()
                .CreateLogger();
        }

        public override async Task<Task> StartAsync(CancellationToken cancellationToken)
        {
            var _logAction = "StartAsync";
            _assemblyVersion = typeof(Worker).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            Log.Information("{logAction}: Starting version {version}", _logAction, _assemblyVersion);

            #region Discord Client
            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = _config["Discord:Token"],
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            });

            Client.Ready += Client_Ready;
            Client.GuildAvailable += Client_GuildAvailable;
            Client.ClientErrored += Client_ClientError;

            var deps = new ServiceCollection()
                .AddSingleton(typeof(Services), new Services(_config, Client, Commands))
                .AddSingleton(typeof(IConfiguration), _config)
                .BuildServiceProvider();
            #endregion

            #region Commands
            Commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { _config["Discord:CommandPrefix"] },
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = deps
            });

            Commands.CommandExecuted += Commands_CommandExecuted;
            Commands.CommandErrored += Commands_CommandErrored;

            Commands.RegisterCommands<MessageCommands>();

            #endregion

            await Client.ConnectAsync();



            await Task.Delay(-1);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            // let's log the fact that this event occured
            sender.Logger.LogInformation(BotEventId, "Client is ready to process events.");

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
        {
            // let's log the name of the guild that was just
            // sent to our client
            sender.Logger.LogInformation(BotEventId, $"Guild available: {e.Guild.Name}");

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private Task Client_ClientError(DiscordClient sender, ClientErrorEventArgs e)
        {
            // let's log the details of the error that just 
            // occured in our client
            sender.Logger.LogError(BotEventId, e.Exception, "Exception occured");

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            // let's log the name of the command and user
            e.Context.Client.Logger.LogInformation(BotEventId, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'");

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            // let's log the error details
            e.Context.Client.Logger.LogError(BotEventId, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            // let's check if the error is a result of lack
            // of required permissions
            if (e.Exception is ChecksFailedException ex)
            {
                // yes, the user lacks required permissions, 
                // let them know

                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                // let's wrap the response into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // red
                };
                await e.Context.RespondAsync("", embed: embed);
            }
        }
    }
}
