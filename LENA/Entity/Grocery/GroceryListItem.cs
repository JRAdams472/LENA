using System;

using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Grocery
{
    public class GroceryListItem : AuditableEntity
    {
        public int GroceryListItemID { get; set; }
        public int GroceryListID { get; set; }
        public int? ItemID { get; set; }
        public string? ItemName { get; set; }
        public string? ManualItemName { get; set; }
        public decimal QuantityNeeded { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string Source { get; set; } = string.Empty;
        public bool IsChecked { get; set; }

        public GroceryList? GroceryList { get; set; }
    }
}