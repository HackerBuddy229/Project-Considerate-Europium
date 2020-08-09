using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Communication
{
    public class RegistrationRequest
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string UserName { get; set; }


        public string Password { get; set; }
    }
}
