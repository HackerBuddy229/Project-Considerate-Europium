using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models.Authentication
{
    public class AuthenticationToken
    {
        public string AuthenticationBearerToken { get; set; }
        public string AuthenticationTokenExpires { get; set; }
    }
}
