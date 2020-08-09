using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Communication
{
    public class RegistrationResponse
    {
        public RegistrationRequest Request { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public bool Succeeded => !Errors.Any();
    }
}
