using System;
using System.IO;
using FluentAssertions;
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

            directory.Should().NotBeNull("the LENA.Database project must be locatable from the test output directory");

            var path = Path.Combine(directory!.FullName, "LENA.Database", Path.Combine(relativePath));
            File.Exists(path).Should().BeTrue($"{path} should exist");
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
            updates.Length.Should().BeGreaterThan(1);
            AdjustQuantity.Should().Contain("@LastUpdatedBy NVARCHAR(100)");

            foreach (var update in updates[1..])
            {
                update.Should().Contain("[CurrentQuantity] = @Quantity");
                update.Should().Contain("[LastUpdatedDate] = SYSUTCDATETIME()");
                update.Should().Contain("[LastUpdatedBy] = @LastUpdatedBy");
            }
        }

        [Fact]
        public void Generate_Should_Surface_Items_Depleted_Since_The_Previous_List()
        {
            GenerateFromMealPlan.Should().Contain("DECLARE @LastGeneratedDate DATETIME2 = (SELECT MAX(GeneratedDate) FROM [MealPlan].[GroceryList])");
            GenerateFromMealPlan.Should().Contain("'Depleted'");
            GenerateFromMealPlan.Should().Contain("WHERE i.CurrentQuantity = 0 AND (@LastGeneratedDate IS NULL OR i.LastUpdatedDate > @LastGeneratedDate)");
        }

        [Fact]
        public void Generate_Should_Net_On_Hand_Quantity_Only_Within_The_Inventory_Unit()
        {
            // every PlanItems branch keeps the line's own unit of measure in the grouping key
            GenerateFromMealPlan.Should().Contain("GROUP BY ri.ItemID, ri.UnitOfMeasure");
            GenerateFromMealPlan.Should().Contain("GROUP BY msi.ItemID, msi.UnitOfMeasure");

            // on-hand inventory is subtracted only from the group expressed in the item's own unit
            GenerateFromMealPlan.Should().Contain(
                "SUM(p.TotalNeeded) - CASE WHEN COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit) = i.Unit THEN i.CurrentQuantity ELSE 0 END AS QuantityNeeded");
            GenerateFromMealPlan.Should().Contain(
                "GROUP BY p.ItemID, i.CurrentQuantity, i.Unit, COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit)");
        }

        [Fact]
        public void Generate_Should_Scale_Recipe_Ingredients_By_Slot_Servings()
        {
            GenerateFromMealPlan.Should().Contain(
                "SUM(ri.Quantity / CAST(ISNULL(NULLIF(r.Servings, 0), 1) AS DECIMAL(10, 2)) * s.Servings)");
        }

        [Theory]
        [InlineData(nameof(GenerateFromMealPlan))]
        [InlineData(nameof(GetItemsByListId))]
        public void Item_Rows_Should_Expose_The_Joined_Inventory_Name(string procedure)
        {
            var sql = procedure == nameof(GenerateFromMealPlan) ? GenerateFromMealPlan : GetItemsByListId;

            sql.Should().Contain("i.Name AS ItemName");
            sql.Should().Contain("LEFT JOIN [Inventory].[Item] i ON gli.ItemID = i.ItemID");
            sql.Should().Contain("ORDER BY gli.Source, COALESCE(i.Name, gli.ManualItemName)");
        }
    }
}
