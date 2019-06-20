using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SCTM.Q.Models
{
    public class DiscordVerifyRequest
    {
        [Required(ErrorMessage = "DiscordVerifyRequest > DiscordId is required")]
        public ulong DiscordId { get; set; }

        [Required(ErrorMessage = "DiscordVerifyRequest > HelloCode is required"), StringLength(5, MinimumLength = 5, ErrorMessage = "DiscordVerifyRequest > HelloCode must be 5 chars")]
        public string HelloCode { get; set; }
    }
}
