using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCTM.Q.Commands
{
    public partial class Commands : ModuleBase
    {
        [Command("roles")]
        public async Task Process_RoleAdd()
        {
            IUser _author = Context.Message.Author;
            var channel = await Context.Message.Author.GetOrCreateDMChannelAsync();

            List<IRole> _roles = Context.Guild.Roles.ToList();

            foreach (var role in _roles)
            {
                // get role manager
                IGuildUser _owner = await Context.Guild.GetUserAsync(ulong.Parse("238837208029724674"));
                string _ownerKnownAs = _owner.Nickname ?? $"{_owner.Username}#{_owner.Discriminator}";

                var _ret = new EmbedBuilder();
                _ret.WithTitle(role.Name);
                _ret.WithDescription("Write out some description of this role");
                _ret.WithColor(role.Color);
                _ret.WithFooter(new EmbedFooterBuilder
                {
                    IconUrl = _owner.GetAvatarUrl() ?? _owner.GetDefaultAvatarUrl(),
                    Text = $"{_ownerKnownAs} manages this role."
                });

                await Context.Message.Channel.SendMessageAsync("", false, _ret.Build());
            }
        }

        [Command("roles add")]
        public async Task Process_RoleAdd(ulong discordId, string roleName)
        {
            IUser _author = Context.Message.Author;
            var channel = await Context.Message.Author.GetOrCreateDMChannelAsync();

            IGuildUser _member = await Context.Guild.GetUserAsync(ulong.Parse(discordId.ToString()));
            string _memberKnownAs = _member.Nickname ?? $"{_member.Username}#{_member.Discriminator}";
            _memberKnownAs += $" [{_member.Id}]";

            
            IRole _role = Context.Guild.Roles.Where(i => i.Name.ToLower() == roleName.ToLower().Trim()).FirstOrDefault();
            if(_role != null)
            {
                await _member.AddRoleAsync(_role);

                await channel.SendMessageAsync($"I've added {_member.Username}#{_member.Discriminator} [{_member.Id}] to role: {_role.Name}");
            } else
            {
                await channel.SendMessageAsync($"I'm not seeing a role named: \"{roleName}\"");
            }
        }

        [Command("roles remove")]
        public async Task Process_RoleRemove(long discordId, string roleName)
        {
            IUser _author = Context.Message.Author;
            var channel = await Context.Message.Author.GetOrCreateDMChannelAsync();

            IGuildUser _member = await Context.Guild.GetUserAsync(ulong.Parse(discordId.ToString()));
            string _memberKnownAs = _member.Nickname ?? $"{_member.Username}#{_member.Discriminator}";
            _memberKnownAs += $" [{_member.Id}]";


            IRole _role = Context.Guild.Roles.Where(i => i.Name.ToLower() == roleName.ToLower().Trim()).FirstOrDefault();
            if (_role != null)
            {
                await _member.RemoveRoleAsync(_role);

                await channel.SendMessageAsync($"I've removed {_member.Username}#{_member.Discriminator} [{_member.Id}] from role: {_role.Name}");
            }
            else
            {
                await channel.SendMessageAsync($"I'm not seeing a role named: \"{roleName}\"");
            }
        }
    }
}
