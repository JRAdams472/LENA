using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class Bottle : AuditableEntity
    {
        public int BottleID { get; set; }
        public int? BottleNumber { get; set; }
        public int TypeID { get; set; }
        public int CountryID { get; set; }
        public int RegionID { get; set; }
        public int VintageYear { get; set; }
        public string? Vineyard { get; set; }
        public decimal? ABV { get; set; }
        public int? Acidity { get; set; }           // Numeric scale 1–5 (Low to Crisp)
        public int? TanninLevel { get; set; }       // Numeric scale 1–5 (None to High)
        public int? Body { get; set; }              // Numeric scale 1–5 (Light to Full)
        public int? Sweetness { get; set; }         // Numeric scale 1–5 (Bone-Dry to Sweet)
        public bool? OakIntegration { get; set; }   // Boolean flag for oak aging
        public string BottleSize { get; set; } = "750ml";
        public int Quantity { get; set; } = 1;
        public DateTime PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? StorageTemp { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public bool IsFavorite { get; set; } = false;

        // Navigation properties
        public Type? Type { get; set; }
        public Country? Country { get; set; }
        public Region? Region { get; set; }
        public Vintage? Vintage { get; set; }
        public ICollection<BottleGrapeVariety> BottleGrapeVarieties { get; set; } = new List<BottleGrapeVariety>();
        public ICollection<BottleFlavorProfile> BottleFlavorProfiles { get; set; } = new List<BottleFlavorProfile>();
    }
}
