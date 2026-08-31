using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Wine
{
    public class BottleFlavorProfile : AuditableEntity
    {
        public int FlavorProfileID { get; set; }
        public string FlavorProfileName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}