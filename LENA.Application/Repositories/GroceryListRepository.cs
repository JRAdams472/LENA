using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Grocery;
using System.Data;

namespace LENA.Application.Repositories
{
    public class GroceryListRepository : BaseRepository<GroceryList>, IGroceryListRepository
    {
        public GroceryListRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<GroceryList> CreateAsync(GroceryList entity, CancellationToken cancellationToken = default)
        {
            entity.GroceryListID = await QuerySingleAsync<int>("[MealPlan].[usp_GroceryList_Create]", new
            {
                entity.MealPlanID,
                entity.GeneratedDate,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<GroceryList?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var groceryList = await connection.QueryFirstOrDefaultAsync<GroceryList>(
                "[MealPlan].[usp_GroceryList_GetById]",
                new { GroceryListID = id },
                commandType: CommandType.StoredProcedure);

            if (groceryList == null)
                return null;

            var items = await QueryListAsync<GroceryListItem>(
                "[MealPlan].[usp_GroceryListItem_GetByGroceryListId]",
                new { GroceryListID = id },
                cancellationToken);

            groceryList.GroceryListItems = new List<GroceryListItem>(items);
            return groceryList;
        }

        public override Task<GroceryList?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => Task.FromResult<GroceryList?>(null);

        public override async Task<IReadOnlyList<GroceryList>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<GroceryList>("[MealPlan].[usp_GroceryList_ListAll]", cancellationToken: cancellationToken);

        public override Task<GroceryList> UpdateAsync(GroceryList entity, CancellationToken cancellationToken = default)
            => throw new NotSupportedException("Grocery lists are not updated directly; regenerate or modify list items.");

        public override async Task<GroceryList> DeleteAsync(GroceryList entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_GroceryList_Delete]", new { entity.GroceryListID }, nameof(GroceryList), entity.GroceryListID, cancellationToken);
            return entity;
        }

        public async Task<GroceryList> GenerateFromMealPlanAsync(GroceryList groceryList, CancellationToken cancellationToken = default)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(
                "[MealPlan].[usp_GroceryList_GenerateFromMealPlan]",
                new { groceryList.MealPlanID, groceryList.CreatedBy, groceryList.CreateDate },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            using var reader = await connection.QueryMultipleAsync(command);
            var result = await reader.ReadSingleAsync<GroceryList>();
            var items = await reader.ReadAsync<GroceryListItem>();
            result.GroceryListItems = new List<GroceryListItem>(items);
            return result;
        }

        public async Task<GroceryListItem> AddGroceryListItemAsync(GroceryListItem groceryListItem, CancellationToken cancellationToken = default)
        {
            groceryListItem.GroceryListItemID = await QuerySingleAsync<int>("[MealPlan].[usp_GroceryListItem_Create]", new
            {
                groceryListItem.GroceryListID,
                groceryListItem.ItemID,
                groceryListItem.ManualItemName,
                groceryListItem.QuantityNeeded,
                groceryListItem.UnitOfMeasure,
                groceryListItem.Source,
                groceryListItem.CreatedBy,
                groceryListItem.CreateDate
            }, cancellationToken);
            return groceryListItem;
        }

        public async Task<GroceryListItem> ToggleGroceryListItemCheckedAsync(GroceryListItem groceryListItem, CancellationToken cancellationToken = default)
        {
            groceryListItem.IsChecked = (await QuerySingleAsync<int>("[MealPlan].[usp_GroceryListItem_ToggleChecked]", new
            {
                groceryListItem.GroceryListItemID,
                groceryListItem.LastUpdatedBy,
                groceryListItem.LastUpdatedDate
            }, cancellationToken)) == 1;
            return groceryListItem;
        }

        public async Task DeleteGroceryListItemAsync(int groceryListItemId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[MealPlan].[usp_GroceryListItem_Delete]", new { GroceryListItemID = groceryListItemId }, nameof(GroceryListItem), groceryListItemId, cancellationToken);
    }
}
