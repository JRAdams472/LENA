using Dapper;
using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Grocery;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LENA.Infrastructure.Persistence
{
    public class GroceryListRepository : BaseRepository<GroceryList>, IGroceryListRepository
    {
        private readonly ICurrentUserService _currentUser;

        public GroceryListRepository(IDbConnectionFactory connectionFactory, ICurrentUserService currentUser) : base(connectionFactory)
        {
            _currentUser = currentUser;
        }

        public override async Task<GroceryList> CreateAsync(GroceryList entity, CancellationToken cancellationToken = default)
        {
            entity.GroceryListID = await QuerySingleAsync<int>("[MealPlan].[usp_GroceryList_Create]", new
            {
                entity.MealPlanID,
                entity.GeneratedDate,
                UserID = _currentUser.UserID,
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
                new { GroceryListID = id, UserID = _currentUser.UserID },
                commandType: CommandType.StoredProcedure);

            if (groceryList == null)
                return null;

            var items = await QueryListAsync<GroceryListItem>(
                "[MealPlan].[usp_GroceryListItem_GetByGroceryListId]",
                new { GroceryListID = id, UserID = _currentUser.UserID },
                cancellationToken);

            groceryList.GroceryListItems = new List<GroceryListItem>(items);
            return groceryList;
        }


        public override async Task<IReadOnlyList<GroceryList>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<GroceryList>("[MealPlan].[usp_GroceryList_ListAll]", new { UserID = _currentUser.UserID }, cancellationToken);

        public override Task<GroceryList> UpdateAsync(GroceryList entity, CancellationToken cancellationToken = default)
            => throw new NotSupportedException("Grocery lists are not updated directly; regenerate or modify list items.");

        public override async Task<GroceryList> DeleteAsync(GroceryList entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_GroceryList_Delete]", new { entity.GroceryListID, UserID = _currentUser.UserID }, nameof(GroceryList), entity.GroceryListID, cancellationToken);
            return entity;
        }

        public async Task<GroceryList> GenerateFromMealPlanAsync(GroceryList groceryList, CancellationToken cancellationToken = default)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            var command = new CommandDefinition(
                "[MealPlan].[usp_GroceryList_GenerateFromMealPlan]",
                new { groceryList.MealPlanID, UserID = _currentUser.UserID, groceryList.CreatedBy, groceryList.CreateDate },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            try
            {
                using var reader = await connection.QueryMultipleAsync(command);
                var result = await reader.ReadSingleAsync<GroceryList>();
                var items = await reader.ReadAsync<GroceryListItem>();
                result.GroceryListItems = new List<GroceryListItem>(items);
                return result;
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                throw new NotFoundException("MealPlan", groceryList.MealPlanID ?? 0);
            }
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
                UserID = _currentUser.UserID,
                groceryListItem.CreatedBy,
                groceryListItem.CreateDate
            }, cancellationToken);

            if (groceryListItem.GroceryListItemID == 0)
                throw new NotFoundException(nameof(GroceryList), groceryListItem.GroceryListID);

            return groceryListItem;
        }

        public async Task<GroceryListItem> ToggleGroceryListItemCheckedAsync(GroceryListItem groceryListItem, CancellationToken cancellationToken = default)
        {
            groceryListItem.IsChecked = (await QuerySingleAsync<int>("[MealPlan].[usp_GroceryListItem_ToggleChecked]", new
            {
                groceryListItem.GroceryListItemID,
                UserID = _currentUser.UserID,
                groceryListItem.LastUpdatedBy,
                groceryListItem.LastUpdatedDate
            }, cancellationToken)) == 1;
            return groceryListItem;
        }

        public async Task DeleteGroceryListItemAsync(int groceryListItemId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[MealPlan].[usp_GroceryListItem_Delete]", new { GroceryListItemID = groceryListItemId, UserID = _currentUser.UserID }, nameof(GroceryListItem), groceryListItemId, cancellationToken);
    }
}
