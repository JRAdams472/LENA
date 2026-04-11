using LENA.Domain.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LENA.Domain.Entity.Inventory
{
    public class Item : AuditableEntity
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string? Brand { get; set; }
        public string? Barcode { get; set; }
        public int CategoryID { get; set; }
        public string Unit { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal? MinQuantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Notes { get; set; }
        public bool IsFavorite { get; set; } = false;

        // Navigation property
        public Category? Category { get; set; }
    }
}
