using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectConsiderateEuropium.Server.Data.Models
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string CreatedAt { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        public string Contributions { get; set; }
        public string RefreshTokenGeneration { get; set; } = Guid.NewGuid().ToString();

    }
}
