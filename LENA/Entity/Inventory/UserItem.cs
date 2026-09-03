using System;

using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Inventory
{
    public class UserItem : AuditableEntity
    {
        public int UserItemID { get; set; }
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal? MinQuantity { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Notes { get; set; }
        public bool IsFavorite { get; set; }

        // Navigation properties
        public Item? Item { get; set; }
    }
}