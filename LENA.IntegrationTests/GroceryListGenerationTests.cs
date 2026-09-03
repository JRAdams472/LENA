using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using FluentAssertions;

using Microsoft.Data.SqlClient;

using Xunit;

namespace LENA.IntegrationTests
{
    public sealed class GroceryListGenerationTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GroceryListGenerationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            Skip.IfNot(_fixture.IsAvailable, "LocalDB is not available.");
            var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();
            return connection;
        }

        private async Task ResetStateAsync(SqlConnection connection)
        {
            // Neutralize items left in a depleted state by earlier tests so they do not leak
            // into the current test's depleted-item checks. GroceryList rows are also cleared
            // so @LastGeneratedDate starts fresh for time-based depletion assertions.
            await connection.ExecuteAsync(
                "UPDATE [Inventory].[UserItem] SET CurrentQuantity = 1000, LastUpdatedDate = '2000-01-01', LastUpdatedBy = 'test' WHERE CurrentQuantity = 0;");
            await connection.ExecuteAsync("DELETE FROM [MealPlan].[GroceryListItem];");
            await connection.ExecuteAsync("DELETE FROM [MealPlan].[GroceryList];");
        }

        private static async Task<int> InsertUserAsync(SqlConnection connection)
        {
            var subject = $"sub-{Guid.NewGuid():N}";
            var email = $"test-{Guid.NewGuid():N}@example.com";
            return await connection.QuerySingleAsync<int>(
                "INSERT INTO [Identity].[User] (ExternalSubject, Provider, Email, DisplayName, CreatedBy, CreateDate) OUTPUT INSERTED.UserID VALUES (@subject, 'google', @email, 'Integration Test', 'test', SYSUTCDATETIME());",
                new { subject, email });
        }

        private static async Task<int> InsertCategoryAsync(SqlConnection connection)
        {
            return await connection.QuerySingleAsync<int>(
                "INSERT INTO [Inventory].[Category] (CategoryName, CreatedBy, CreateDate) OUTPUT INSERTED.CategoryID VALUES (@name, 'test', SYSUTCDATETIME());",
                new { name = $"Category-{Guid.NewGuid():N}" });
        }

        private static async Task<int> InsertItemAsync(
            SqlConnection connection,
            int userId,
            int categoryId,
            string name,
            string unit,
            decimal currentQuantity,
            decimal? minQuantity = null,
            DateTime? lastUpdatedDate = null)
        {
            var uniqueName = $"{name}-{Guid.NewGuid():N}";
            var upc12 = Guid.NewGuid().ToString("N")[..12];
            var upc14 = Guid.NewGuid().ToString("N")[..14];
            var lastUpdatedBy = lastUpdatedDate.HasValue ? "test" : (string?)null;
            var createDate = lastUpdatedDate?.AddSeconds(-1) ?? DateTime.UtcNow;

            var itemId = await connection.QuerySingleAsync<int>(
                @"INSERT INTO [Inventory].[Item]
                  (Name, BrandID, UPC12, UPC14, CategoryID, Unit, CreatedBy, CreateDate)
                  OUTPUT INSERTED.ItemID
                  VALUES (@uniqueName, NULL, @upc12, @upc14, @categoryId, @unit, 'test', @createDate);",
                new { uniqueName, upc12, upc14, categoryId, unit, createDate });

            await connection.ExecuteAsync(
                @"INSERT INTO [Inventory].[UserItem]
                  (UserID, ItemID, CurrentQuantity, MinQuantity, PurchaseDate, ExpiryDate, Notes, IsFavorite, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate)
                  VALUES (@userId, @itemId, @currentQuantity, @minQuantity, SYSUTCDATETIME(), NULL, NULL, 0, 'test', @createDate, @lastUpdatedBy, @lastUpdatedDate);",
                new { userId, itemId, currentQuantity, minQuantity, createDate, lastUpdatedBy, lastUpdatedDate });

            return itemId;
        }

        private static async Task<int> InsertRecipeAsync(SqlConnection connection, int servings)
        {
            return await connection.QuerySingleAsync<int>(
                "INSERT INTO [Recipe].[Recipe] (RecipeName, Servings, CreatedBy, CreateDate) OUTPUT INSERTED.RecipeID VALUES (@name, @servings, 'test', SYSUTCDATETIME());",
                new { name = $"Recipe-{Guid.NewGuid():N}", servings });
        }

        private static async Task InsertRecipeItemAsync(
            SqlConnection connection,
            int recipeId,
            int itemId,
            decimal quantity,
            string unit,
            bool isOptional)
        {
            await connection.ExecuteAsync(
                "INSERT INTO [Recipe].[RecipeItem] (RecipeID, ItemID, Quantity, UnitOfMeasure, IsOptional) VALUES (@recipeId, @itemId, @quantity, @unit, @isOptional);",
                new { recipeId, itemId, quantity, unit, isOptional });
        }

        private static async Task<int> InsertMealPlanAsync(SqlConnection connection, int userId)
        {
            return await connection.QuerySingleAsync<int>(
                "INSERT INTO [MealPlan].[MealPlan] (PlanName, UserID, WeekStartDate, WeekStartDayOfWeek, IsActive, CreatedBy, CreateDate) OUTPUT INSERTED.MealPlanID VALUES (@name, @userId, @weekStart, 0, 1, 'test', SYSUTCDATETIME());",
                new { name = $"Plan-{Guid.NewGuid():N}", userId, weekStart = DateTime.UtcNow.Date });
        }

        private static async Task<int> InsertMealSlotAsync(
            SqlConnection connection,
            int mealPlanId,
            int? recipeId,
            decimal servings)
        {
            return await connection.QuerySingleAsync<int>(
                "INSERT INTO [MealPlan].[MealSlot] (MealPlanID, DayOfWeek, MealType, RecipeID, Servings, CreatedBy, CreateDate) OUTPUT INSERTED.MealSlotID VALUES (@mealPlanId, 0, 0, @recipeId, @servings, 'test', SYSUTCDATETIME());",
                new { mealPlanId, recipeId, servings });
        }

        private static async Task<int> InsertMealSlotItemAsync(
            SqlConnection connection,
            int mealSlotId,
            int itemId,
            decimal quantity,
            string unit,
            bool isFromRecipe)
        {
            return await connection.QuerySingleAsync<int>(
                "INSERT INTO [MealPlan].[MealSlotItem] (MealSlotID, ItemID, Quantity, UnitOfMeasure, IsFromRecipe, CreatedBy, CreateDate) OUTPUT INSERTED.MealSlotItemID VALUES (@mealSlotId, @itemId, @quantity, @unit, @isFromRecipe, 'test', SYSUTCDATETIME());",
                new { mealSlotId, itemId, quantity, unit, isFromRecipe });
        }

        private static async Task<IReadOnlyList<GroceryListItemResult>> GenerateGroceryListAsync(
            SqlConnection connection,
            int userId,
            int mealPlanId,
            DateTime createDate)
        {
            using var multi = await connection.QueryMultipleAsync(
                "[MealPlan].[usp_GroceryList_GenerateFromMealPlan]",
                new { MealPlanID = mealPlanId, UserID = userId, CreatedBy = "test", CreateDate = createDate },
                commandType: CommandType.StoredProcedure);

            _ = await multi.ReadSingleAsync<GroceryListResult>();
            return (await multi.ReadAsync<GroceryListItemResult>()).ToList().AsReadOnly();
        }

        [SkippableFact]
        public async Task Netting_Subtracts_CurrentInventory_And_Excludes_NonPositive_Needs()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(connection, userId, categoryId, "Flour", "g", currentQuantity: 5);
            var recipeId = await InsertRecipeAsync(connection, servings: 2);
            await InsertRecipeItemAsync(connection, recipeId, itemId, quantity: 10, unit: "g", isOptional: false);

            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            await InsertMealSlotAsync(connection, mealPlanId, recipeId, servings: 2);

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, DateTime.UtcNow);

            items.Should().ContainSingle();
            items[0].ItemID.Should().Be(itemId);
            items[0].QuantityNeeded.Should().Be(5);
            items[0].UnitOfMeasure.Should().Be("g");
            items[0].Source.Should().Be("MealPlan");
        }

        [SkippableFact]
        public async Task Netting_Keeps_Different_Units_Separate()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(connection, userId, categoryId, "Milk", "g", currentQuantity: 5);
            var recipeId = await InsertRecipeAsync(connection, servings: 2);
            await InsertRecipeItemAsync(connection, recipeId, itemId, quantity: 10, unit: "g", isOptional: false);

            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            var slotId = await InsertMealSlotAsync(connection, mealPlanId, recipeId, servings: 2);
            await InsertMealSlotItemAsync(connection, slotId, itemId, quantity: 3, unit: "cup", isFromRecipe: false);

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, DateTime.UtcNow);

            items.Should().HaveCount(2);
            items.Should().ContainSingle(i => i.UnitOfMeasure == "g" && i.QuantityNeeded == 5);
            items.Should().ContainSingle(i => i.UnitOfMeasure == "cup" && i.QuantityNeeded == 3);
        }

        [SkippableFact]
        public async Task Optional_Recipe_Items_Are_Included_When_Selected()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(connection, userId, categoryId, "Cheese", "g", currentQuantity: 0);
            var recipeId = await InsertRecipeAsync(connection, servings: 2);
            await InsertRecipeItemAsync(connection, recipeId, itemId, quantity: 10, unit: "g", isOptional: true);

            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            var slotId = await InsertMealSlotAsync(connection, mealPlanId, recipeId, servings: 2);
            await InsertMealSlotItemAsync(connection, slotId, itemId, quantity: 0, unit: "g", isFromRecipe: true);

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, DateTime.UtcNow);

            items.Should().ContainSingle();
            items[0].QuantityNeeded.Should().Be(10);
            items[0].Source.Should().Be("MealPlan");
        }

        [SkippableFact]
        public async Task Optional_Recipe_Items_Are_Not_Included_When_Not_Selected()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(connection, userId, categoryId, "Cheese", "g", currentQuantity: 10);
            var recipeId = await InsertRecipeAsync(connection, servings: 2);
            await InsertRecipeItemAsync(connection, recipeId, itemId, quantity: 10, unit: "g", isOptional: true);

            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            await InsertMealSlotAsync(connection, mealPlanId, recipeId, servings: 2);

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, DateTime.UtcNow);

            items.Should().BeEmpty();
        }

        [SkippableFact]
        public async Task Ad_Hoc_Slot_Items_Are_Included()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(connection, userId, categoryId, "Bananas", "each", currentQuantity: 0);

            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            var slotId = await InsertMealSlotAsync(connection, mealPlanId, recipeId: null, servings: 1);
            await InsertMealSlotItemAsync(connection, slotId, itemId, quantity: 3, unit: "each", isFromRecipe: false);

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, DateTime.UtcNow);

            items.Should().ContainSingle();
            items[0].QuantityNeeded.Should().Be(3);
            items[0].UnitOfMeasure.Should().Be("each");
            items[0].Source.Should().Be("MealPlan");
        }

        [SkippableFact]
        public async Task Depleted_Items_Are_Added_When_Updated_Since_Last_Generation()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            var baseline = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // First generation - nothing depleted yet.
            await GenerateGroceryListAsync(connection, userId, mealPlanId, baseline);

            // Item depleted after the first list was generated.
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(
                connection,
                userId,
                categoryId,
                "Eggs",
                "each",
                currentQuantity: 0,
                minQuantity: 2,
                lastUpdatedDate: baseline.AddHours(1));

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, baseline.AddHours(2));

            items.Should().ContainSingle();
            items[0].ItemID.Should().Be(itemId);
            items[0].QuantityNeeded.Should().Be(2);
            items[0].Source.Should().Be("Depleted");
        }

        [SkippableFact]
        public async Task Depleted_Items_Are_Not_Added_When_LastUpdated_Is_Before_Last_Generation()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            var baseline = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // First generation establishes a baseline.
            await GenerateGroceryListAsync(connection, userId, mealPlanId, baseline);

            // Item was depleted before the baseline, so it should not re-appear.
            var categoryId = await InsertCategoryAsync(connection);
            _ = await InsertItemAsync(
                connection,
                userId,
                categoryId,
                "Butter",
                "g",
                currentQuantity: 0,
                minQuantity: 1,
                lastUpdatedDate: baseline.AddHours(-1));

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, baseline.AddHours(1));

            items.Should().BeEmpty();
        }

        [SkippableFact]
        public async Task Depleted_Items_Are_Not_Duplicated_When_Already_In_Current_List()
        {
            await using var connection = await GetOpenConnectionAsync();
            await ResetStateAsync(connection);

            var userId = await InsertUserAsync(connection);
            var categoryId = await InsertCategoryAsync(connection);
            var itemId = await InsertItemAsync(connection, userId, categoryId, "Oats", "g", currentQuantity: 0);
            var recipeId = await InsertRecipeAsync(connection, servings: 1);
            await InsertRecipeItemAsync(connection, recipeId, itemId, quantity: 10, unit: "g", isOptional: false);

            var mealPlanId = await InsertMealPlanAsync(connection, userId);
            await InsertMealSlotAsync(connection, mealPlanId, recipeId, servings: 1);

            var items = await GenerateGroceryListAsync(connection, userId, mealPlanId, DateTime.UtcNow);

            items.Should().ContainSingle();
            items[0].Source.Should().Be("MealPlan");
            items[0].QuantityNeeded.Should().Be(10);
        }

        private sealed class GroceryListResult
        {
            public int GroceryListID { get; set; }
        }

        private sealed class GroceryListItemResult
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
        }
    }
}