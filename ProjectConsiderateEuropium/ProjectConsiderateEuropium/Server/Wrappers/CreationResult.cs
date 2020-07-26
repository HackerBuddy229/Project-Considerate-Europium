using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectConsiderateEuropium.Server.Wrappers
{
    public class CreationResult<T>
    {
        public T Created { get; set; }
        
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public bool Succeeded => !Errors.Any();
    }
}
