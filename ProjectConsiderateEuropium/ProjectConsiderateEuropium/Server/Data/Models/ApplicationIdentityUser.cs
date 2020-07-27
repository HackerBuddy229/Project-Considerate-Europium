using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectConsiderateEuropium.Server.Data.Models
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string CreatedAt { get; set; }

        public string Contributions { get; set; }
        public string RefreshTokenGeneration { get; set; }

    }
}
