using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.Configuration;
using System;

namespace sctm.discord
{
    public partial class Services
    {
        public DiscordEmoji FailEmoji;
        public DiscordEmoji SearchEmoji;

        private IConfiguration _config;
        private DiscordClient _discord;
        private CommandsNextExtension _commands;
        
        string _token = null;
        DateTime _tokenDate = DateTime.MinValue;
        

        public Services(IConfiguration config, DiscordClient discord, CommandsNextExtension commands)
        {
            _config = config;
            _discord = discord;
            _commands = commands;

            SearchEmoji = DiscordEmoji.FromName(discord, ":mag:");
            FailEmoji = DiscordEmoji.FromName(discord, ":cry:");
        }
    }
}
