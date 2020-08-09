using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ProjectConsiderateEuropium.Shared.Models.Authentication;

namespace ProjectConsiderateEuropium.Shared.Communication
{
    public class AuthenticationResponse
    {
        public TokenSet Tokens { get; set; } = null;

        public bool Succeeded => Tokens != null;

        public string CreatedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}
