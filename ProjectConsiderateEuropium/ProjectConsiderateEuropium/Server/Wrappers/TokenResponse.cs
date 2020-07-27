using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectConsiderateEuropium.Server.Wrappers
{
    public class TokenResponse
    {
        public string TokenCreated { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public bool Succeeded => Errors.Any();
    }
}
