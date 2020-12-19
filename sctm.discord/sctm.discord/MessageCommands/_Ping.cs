using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Serilog;
using System.Threading.Tasks;

namespace sctm.discord
{
    public partial class MessageCommands : BaseCommandModule
    {
        [Command("ping")] // let's define this method as a command
        [Description("Ping!")] // this will be displayed to tell users what this command does when they invoke help
        [Aliases("pong")] // alternative names for the command
        public async Task Ping(CommandContext ctx) // this command takes no arguments
        {
            var _logAction = $"MessageCommands_Ping > {ctx.Message.Author.Id}";
            Log.Information("{logAction}: Command called", _logAction);

            await ctx.TriggerTypingAsync();

            // let's make the message a bit more colourful
            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            // respond with ping
            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms uid: {ctx.Message.Author.Id}");
        }
    }
}
