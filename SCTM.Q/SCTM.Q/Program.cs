using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using SCTM.Q.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SCTM.Q
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _handler = new CommandHandler();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("SCTM_DiscordToken"));
            await _client.StartAsync();

            await _handler.Install(_client);

            _client.Ready += Client_Ready;
            await Task.Delay(-1);
        }

        private async Task Client_Ready()
        {
            Console.WriteLine("The Bot is online");
            return;
        }
    }

}
