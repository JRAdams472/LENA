using System;
using System.Collections.Generic;
using System.Text;

using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class GrapeVariety : AuditableEntity
    {
        public int GrapeVarietyID { get; set; }
        public required string GrapeVarietyName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<BottleGrapeVariety>? BottleGrapeVarieties { get; set; }
    }
}