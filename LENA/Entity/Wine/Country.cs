using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Wine
{
    public class Country : AuditableEntity
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string ISOCode { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Region> Regions { get; set; } = new List<Region>();
        public ICollection<Bottle> Bottles { get; set; } = new List<Bottle>();
    }
}
