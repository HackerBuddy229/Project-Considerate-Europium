using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Communication
{
    public class AuthenticationRequest
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Password { get; set; }

        public bool KeepMeLoggedIn { get; set; }
    }
}
