using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Wine
{
    public class BottleGrapeVariety : AuditableEntity
    {
        public int BottleID { get; set; }
        public int GrapeVarietyID { get; set; }
        public byte? Percentage { get; set; }

        // Navigation properties
        public Bottle Bottle { get; set; }
        public GrapeVariety GrapeVariety { get; set; }
    }
}