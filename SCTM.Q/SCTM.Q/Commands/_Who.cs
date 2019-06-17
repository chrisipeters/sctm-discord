using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace SCTM.Q.Commands
{
    public partial class Commands : ModuleBase
    {
        [Command("who")]
        public async Task Process_Who(SocketGuildUser mention)
        {
            if(mention != null)
            {
                var channel = await mention.GetOrCreateDMChannelAsync();
                //await channel.SendMessageAsync($"{Context.Message.Author.Username} just asked who you are...");
                await ReplyAsync($"({mention.Nickname ?? mention.Username}) - {mention.Username}#{mention.Discriminator} [{mention.Id}]");
            }
        }
    }
}
