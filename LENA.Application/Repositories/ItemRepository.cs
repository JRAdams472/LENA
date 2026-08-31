using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using System.Data;

namespace LENA.Application.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<Item>> ListAllAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return (IReadOnlyList<Item>)await connection.QueryAsync<Item>(
                "[Inventory].[usp_Item_ListAll]",
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Item?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Item>(
                "[Inventory].[usp_Item_GetById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Item?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<Item>(
                "[Inventory].[usp_Item_GetByName]",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
        }

        public override async Task<Item> CreateAsync(Item entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var id = await connection.QuerySingleAsync<int>(
                "[Inventory].[usp_Item_Create]",
                entity,
                commandType: CommandType.StoredProcedure);
            entity.ItemID = id;
            return entity;
        }

        public override async Task<Item> UpdateAsync(Item entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_Update]",
                entity,
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public override async Task<Item> DeleteAsync(Item entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_Delete]",
                new { entity.ItemID },
                commandType: CommandType.StoredProcedure);
            return entity;
        }

        public async Task ChangeItemCategoryAsync(int itemId, int newCategoryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_ChangeItemCategory]",
                new { ItemID = itemId, CategoryID = newCategoryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddOrUpdateUPC12Async(int itemId, string upc12)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_AddOrUpdateUPC12]",
                new { ItemID = itemId, UPC12 = upc12 },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddOrUpdateUPC14Async(int itemId, string upc14)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_AddOrUpdateUPC14]",
                new { ItemID = itemId, UPC14 = upc14 },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_AdjustQuantity]",
                new { ItemID = itemId, Quantity = quantity, PurchaseDate = purchaseDate },
                commandType: CommandType.StoredProcedure);
        }

        public async Task SetFavoriteAsync(int itemId, bool isFavorite)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "[Inventory].[usp_Item_SetFavorite]",
                new { ItemID = itemId, IsFavorite = isFavorite },
                commandType: CommandType.StoredProcedure);
        }
    }
}
