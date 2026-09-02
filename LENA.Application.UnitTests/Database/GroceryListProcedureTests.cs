using System;
using System.IO;

using Xunit;

namespace LENA.Application.UnitTests.Database
{
    /// <summary>
    /// Guards the SQL contracts the grocery list feature depends on: depletion detection needs the
    /// audit stamp written by usp_Item_AdjustQuantity, netting must not mix units, and both result
    /// sets must resolve item names through [Inventory].[Item].
    /// </summary>
    public class GroceryListProcedureTests
    {
        private static string ReadProcedure(params string[] relativePath)
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, "LENA.Database")))
            {
                directory = directory.Parent;
            }

Assert.NotNull(            directory);

            var path = Path.Combine(directory!.FullName, "LENA.Database", Path.Combine(relativePath));
Assert.True(            File.Exists(path));
            return File.ReadAllText(path);
        }

        private static string Normalize(string sql) =>
            string.Join(' ', sql.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

        private static readonly string AdjustQuantity = Normalize(
            ReadProcedure("Inventory", "StoredProcedures", "usp_Item_AdjustQuantity.sql"));

        private static readonly string GenerateFromMealPlan = Normalize(
            ReadProcedure("MealPlan", "StoredProcedures", "usp_GroceryList_GenerateFromMealPlan.sql"));

        private static readonly string GetItemsByListId = Normalize(
            ReadProcedure("MealPlan", "StoredProcedures", "usp_GroceryListItem_GetByGroceryListId.sql"));

        [Fact]
        public void AdjustQuantity_Should_Stamp_Audit_Columns_On_Every_Update()
        {
            var updates = AdjustQuantity.Split("UPDATE [Inventory].[Item]", StringSplitOptions.RemoveEmptyEntries);

            // one leading segment (the header) plus one segment per UPDATE statement
Assert.True(            updates.Length > 1);
Assert.Contains("@LastUpdatedBy NVARCHAR(100)",             AdjustQuantity);

            foreach (var update in updates[1..])
            {
Assert.Contains("[CurrentQuantity] = @Quantity",                 update);
Assert.Contains("[LastUpdatedDate] = SYSUTCDATETIME()",                 update);
Assert.Contains("[LastUpdatedBy] = @LastUpdatedBy",                 update);
            }
        }

        [Fact]
        public void Generate_Should_Surface_Items_Depleted_Since_The_Previous_List()
        {
Assert.Contains("DECLARE @LastGeneratedDate DATETIME2 = (SELECT MAX(GeneratedDate) FROM [MealPlan].[GroceryList])",             GenerateFromMealPlan);
Assert.Contains("'Depleted'",             GenerateFromMealPlan);
            Assert.Contains("WHERE i.CurrentQuantity = 0 AND i.LastUpdatedDate > i.CreateDate AND i.LastUpdatedDate > DATEADD(day, -10, @CreateDate)", GenerateFromMealPlan);
        }

        [Fact]
        public void Generate_Should_Net_On_Hand_Quantity_Only_Within_The_Inventory_Unit()
        {
            // every PlanItems branch keeps the line's own unit of measure in the grouping key
Assert.Contains("GROUP BY ri.ItemID, ri.UnitOfMeasure",             GenerateFromMealPlan);
Assert.Contains("GROUP BY msi.ItemID, msi.UnitOfMeasure",             GenerateFromMealPlan);

            // on-hand inventory is subtracted only from the group expressed in the item's own unit
            Assert.Contains("SUM(p.TotalNeeded) - CASE WHEN COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit) = i.Unit THEN i.CurrentQuantity ELSE 0 END AS QuantityNeeded", GenerateFromMealPlan);
            Assert.Contains("GROUP BY p.ItemID, i.CurrentQuantity, i.Unit, COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit)", GenerateFromMealPlan);
        }

        [Fact]
        public void Generate_Should_Scale_Recipe_Ingredients_By_Slot_Servings()
        {
            Assert.Contains("SUM(ri.Quantity / CAST(ISNULL(NULLIF(r.Servings, 0), 1) AS DECIMAL(10, 2)) * s.Servings)", GenerateFromMealPlan);
        }

        [Theory]
        [InlineData(nameof(GenerateFromMealPlan))]
        [InlineData(nameof(GetItemsByListId))]
        public void Item_Rows_Should_Expose_The_Joined_Inventory_Name(string procedure)
        {
            var sql = procedure == nameof(GenerateFromMealPlan) ? GenerateFromMealPlan : GetItemsByListId;

Assert.Contains("i.Name AS ItemName",             sql);
Assert.Contains("LEFT JOIN [Inventory].[Item] i ON gli.ItemID = i.ItemID",             sql);
Assert.Contains("ORDER BY gli.Source, COALESCE(i.Name, gli.ManualItemName)",             sql);
        }
    }
}
