using System;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.MealPlan
{
    public class MealSlotItem : AuditableEntity
    {
        public int MealSlotItemID { get; set; }
        public int MealSlotID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public string? UnitOfMeasure { get; set; }
        public bool IsFromRecipe { get; set; }

        public MealSlot? MealSlot { get; set; }
        public Inventory.Item? Item { get; set; }
    }
}
