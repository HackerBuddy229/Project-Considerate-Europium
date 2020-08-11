using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models
{
    public class File
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }

        public string Path { get; set; }

        public string Extension => System.IO.Path.GetExtension(Name);
        public bool HasExtension => !string.IsNullOrWhiteSpace(Extension);
    }
}
