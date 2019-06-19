using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SCTM.Q.Commands
{
    public partial class Commands : ModuleBase
    {
        [Command("hello")]
        public async Task Process_Hello(string helloCode)
        {
            var _code = helloCode;
            var _discordUsername = Context.Message.Author.Username;

            var channel = await Context.Message.Author.GetOrCreateDMChannelAsync();

            var msg = await channel.SendMessageAsync($"Hi {_discordUsername}. I'm Verifying {_code} as your hello code...");
            await Context.Message.DeleteAsync();
            Thread.Sleep(5000);

            await channel.SendMessageAsync($"Welcome {_discordUsername}");

        }
    }
}
