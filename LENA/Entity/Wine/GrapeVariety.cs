using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Wine
{
    public class GrapeVariety : AuditableEntity
    {
        public int GrapeVarietyID { get; set; }
        public string GrapeVarietyName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<BottleGrapeVariety> BottleGrapeVarieties { get; set; } = new List<BottleGrapeVariety>();
    }
}