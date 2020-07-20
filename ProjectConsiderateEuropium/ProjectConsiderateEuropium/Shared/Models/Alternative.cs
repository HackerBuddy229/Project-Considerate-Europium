using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectConsiderateEuropium.Shared.Models
{
    public class Alternative
    {
        [Key]
        public string Id { get; set; }
        public string CreatedAt { get; set; }

        //img url
        public string Logo { get; set; }

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

    }
}
