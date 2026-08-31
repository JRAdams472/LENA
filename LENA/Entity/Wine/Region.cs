using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class Region : AuditableEntity
    {
        public int RegionID { get; set; }
        public required string RegionName { get; set; }
        public int CountryID { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Country? Country { get; set; }
        public ICollection<Bottle>? Bottles { get; set; }
    }
}
