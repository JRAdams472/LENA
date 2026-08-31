using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class BottleFlavorProfile : AuditableEntity
    {
        public int FlavorProfileID { get; set; }
        public required string FlavorProfileName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
