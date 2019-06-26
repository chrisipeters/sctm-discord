using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using SCTM.Q.Models;
using System.Linq;
using System.Net.Http;
using System.Text;
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

            var _body = new DiscordVerifyRequest
            {
                DiscordId = Context.Message.Author.Id,
                HelloCode = helloCode.Trim()
            };
            var _result = await _http.PostAsync("auth/verify/discord", new StringContent(JsonConvert.SerializeObject(_body), Encoding.UTF8, "application/json"));
            if(_result.IsSuccessStatusCode)
            {

                IGuildUser _member = await Context.Guild.GetUserAsync(Context.Message.Author.Id);
                string _memberKnownAs = _member.Nickname ?? $"{_member.Username}#{_member.Discriminator}";
                _memberKnownAs += $" [{_member.Id}]";


                IRole _role = Context.Guild.Roles.Where(i => i.Name.ToLower() == "members").FirstOrDefault();
                if (_role != null)
                {
                    await _member.AddRoleAsync(_role);

                    await channel.SendMessageAsync($"I've added {_member.Username}#{_member.Discriminator} [{_member.Id}] to role: {_role.Name}");
                }
                else
                {
                    await channel.SendMessageAsync($"I'm not seeing a role named: members");
                }


                await channel.SendMessageAsync($"Welcome {_discordUsername}");
                
            } else
            {
                await channel.SendMessageAsync($"Sorry {_discordUsername}, something went wrong");
            }
            

        }
    }
}
