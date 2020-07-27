using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models.Authentication
{
    public class RefreshToken
    {
        public string RefreshBearerToken { get; set; }
        public string RefreshTokenExpires { get; set; }
    }
}
