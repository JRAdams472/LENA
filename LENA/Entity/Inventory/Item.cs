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
        public string? UPC12 { get; set; }
        public string? UPC14 { get; set; }
        public int CategoryID { get; set; }
        public string Unit { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal? MinQuantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Notes { get; set; }
        public bool IsFavorite { get; set; } = false;

        // Navigation properties
        public Category? Category { get; set; }
        public ICollection<FoodNutrient>? FoodNutrients { get; set; }
        public ICollection<FoodFlavor>? FoodFlavors { get; set; }
    }
}
