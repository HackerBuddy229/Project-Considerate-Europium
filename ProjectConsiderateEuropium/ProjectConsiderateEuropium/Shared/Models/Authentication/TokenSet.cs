using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models.Authentication
{
    public class TokenSet
    {
        public AuthenticationToken AuthenticationToken { get; set; } = new AuthenticationToken();

        public RefreshToken RefreshToken { get; set; } = new RefreshToken();
    }
}
