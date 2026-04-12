using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

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
        public string? GrapeVariety { get; set; }
        public decimal? ABV { get; set; }
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
    }
}
