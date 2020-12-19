using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sctm.discord
{
    public partial class MessageCommands : BaseCommandModule
    {
        private IConfiguration _config;
        private Services _services;
        private Embeds _embeds;

        public MessageCommands(IConfiguration config, Services services)
        {
            _config = config;
            _services = services;
            _embeds = new Embeds(_config);
        }
    }
}
