using System;
using System.Collections.Generic;
using System.Text;
using ProjectConsiderateEuropium.Shared.Models;

namespace ProjectConsiderateEuropium.Shared.Communication
{
    public class UserEntityCreationRequest<T>
    {
        public File Image { get; set; }
        public T NewEntity { get; set; }
    }
}
