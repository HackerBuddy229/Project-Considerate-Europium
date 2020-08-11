using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models
{
    public class Product
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

        //img url
        public string Logo { get; set; }
        public string AccentColor { get; set; }

        //name
        public string Designation { get; set; }
        public string DesignationType { get; set; }

        //contributor
        public string Contributor { get; set; }
        public string ContributorType { get; set; }

        public string Description { get; set; }

        public string OrganizationIdentifier { get; set; }
        public bool HasOrganizationIdentifier => !string.IsNullOrWhiteSpace(OrganizationIdentifier);

        public string Jurisdiction { get; set; }

        public string AlternativeIds { get; set; }

        [NotMapped]
        public IEnumerable<Alternative> Alternatives { get; set; }
    }
}
