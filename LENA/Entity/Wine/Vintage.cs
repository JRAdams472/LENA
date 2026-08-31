using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class Vintage : AuditableEntity
    {
        public int VintageID { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Bottle>? Bottles { get; set; }
    }
}
