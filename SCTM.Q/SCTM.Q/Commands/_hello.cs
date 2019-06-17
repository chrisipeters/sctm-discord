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
        public async Task Process_Hello()
        {
            var msg = await ReplyAsync("Verifying your hello code...");
            Thread.Sleep(5000);

            await Context.Message.DeleteAsync();
            await msg.DeleteAsync();
        }
    }
}
