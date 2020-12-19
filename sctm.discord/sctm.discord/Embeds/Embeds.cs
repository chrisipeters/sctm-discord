using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sctm.discord
{
    public partial class Embeds
    {
        private IConfiguration _config;

        public Embeds(IConfiguration config)
        {
            _config = config;
        }
    }
}
