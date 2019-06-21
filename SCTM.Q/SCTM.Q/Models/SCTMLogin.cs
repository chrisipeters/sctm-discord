using System;
using System.Collections.Generic;
using System.Text;

namespace SCTM.Q.Models
{
    public class LoginResult
    {
        public string RSIProfiles { get; set; }
        public string RSIProfileIconPath { get; set; }
        public string JWTId { get; set; }
        public string JWTAccess { get; set; }
        public string JWTRefresh { get; set; }
        public string Output { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
