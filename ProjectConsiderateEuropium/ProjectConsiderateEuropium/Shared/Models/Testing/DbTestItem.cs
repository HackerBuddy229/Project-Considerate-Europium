using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models.Testing
{
    public class DbTestItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool State { get; set; }
    }
}
