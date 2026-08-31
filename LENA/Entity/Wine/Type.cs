using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class Type : AuditableEntity
    {
        public int TypeID { get; set; }
        public required string TypeName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Bottle>? Bottles { get; set; }
    }
}
