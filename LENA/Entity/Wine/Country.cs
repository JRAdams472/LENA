using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class Country : AuditableEntity
    {
        public int CountryID { get; set; }
        public required string CountryName { get; set; }
        public required string ISOCode { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Region>? Regions { get; set; }
        public ICollection<Bottle>? Bottles { get; set; }
    }
}
