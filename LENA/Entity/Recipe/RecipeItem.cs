using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Inventory;

namespace LENA.Domain.Entity.Recipe
{
    public class RecipeItem
    {
        public int RecipeID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Notes { get; set; }

        public Recipe? Recipe { get; set; }
        public Item? Item { get; set; }
    }
}
