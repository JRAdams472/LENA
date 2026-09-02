using System;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Wine
{
    public class UserBottle : AuditableEntity
    {
        public int UserBottleID { get; set; }
        public int UserID { get; set; }
        public int BottleID { get; set; }
        public int? BottleNumber { get; set; }
        public string BottleSize { get; set; } = "750ml";
        public int Quantity { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? StorageTemp { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public bool IsFavorite { get; set; }

        public Bottle? Bottle { get; set; }
    }
}
