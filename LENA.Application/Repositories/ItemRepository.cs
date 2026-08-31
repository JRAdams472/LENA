using Dapper;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

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
            var sql = "SELECT * FROM [Inventory].[Item] ORDER BY [Name]";
            return (IReadOnlyList<Item>)await connection.QueryAsync<Item>(sql);
        }

        public override async Task<Item?> GetByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Inventory].[Item] WHERE [ItemID] = @Id";
            return await connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = id });
        }

        public override async Task<Item?> GetByNameAsync(string name)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM [Inventory].[Item] WHERE [Name] = @Name";
            return await connection.QueryFirstOrDefaultAsync<Item>(sql, new { Name = name });
        }

        public override async Task<Item> CreateAsync(Item entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                INSERT INTO [Inventory].[Item]
                ([Name], [Brand], [UPC12], [UPC14], [CategoryID], [Unit], [CurrentQuantity], [MinQuantity],
                 [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
                VALUES
                (@Name, @Brand, @UPC12, @UPC14, @CategoryID, @Unit, @CurrentQuantity, @MinQuantity,
                 @PurchaseDate, @ExpiryDate, @Notes, @IsFavorite, @CreatedBy, @CreateDate);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            var id = await connection.QuerySingleAsync<int>(sql, entity);
            entity.ItemID = id;
            return entity;
        }

        public override async Task<Item> UpdateAsync(Item entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = @"
                UPDATE [Inventory].[Item]
                SET [Name] = @Name, [Brand] = @Brand, [UPC12] = @UPC12, [UPC14] = @UPC14,
                    [CategoryID] = @CategoryID, [Unit] = @Unit, [CurrentQuantity] = @CurrentQuantity,
                    [MinQuantity] = @MinQuantity, [PurchaseDate] = @PurchaseDate, [ExpiryDate] = @ExpiryDate,
                    [Notes] = @Notes, [IsFavorite] = @IsFavorite, [LastUpdatedBy] = @LastUpdatedBy,
                    [LastUpdatedDate] = @LastUpdatedDate
                WHERE [ItemID] = @ItemID";

            await connection.ExecuteAsync(sql, entity);
            return entity;
        }

        public override async Task<Item> DeleteAsync(Item entity)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "DELETE FROM [Inventory].[Item] WHERE [ItemID] = @ItemID";
            await connection.ExecuteAsync(sql, new { entity.ItemID });
            return entity;
        }

        public async Task ChangeItemCategoryAsync(int itemId, int newCategoryId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "UPDATE [Inventory].[Item] SET [CategoryID] = @CategoryID WHERE [ItemID] = @ItemID";
            await connection.ExecuteAsync(sql, new { ItemID = itemId, CategoryID = newCategoryId });
        }

        public async Task AddOrUpdateUPC12Async(int itemId, string upc12)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "UPDATE [Inventory].[Item] SET [UPC12] = @UPC12 WHERE [ItemID] = @ItemID";
            await connection.ExecuteAsync(sql, new { ItemID = itemId, UPC12 = upc12 });
        }

        public async Task AddOrUpdateUPC14Async(int itemId, string upc14)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "UPDATE [Inventory].[Item] SET [UPC14] = @UPC14 WHERE [ItemID] = @ItemID";
            await connection.ExecuteAsync(sql, new { ItemID = itemId, UPC14 = upc14 });
        }

        public async Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = purchaseDate.HasValue
                ? "UPDATE [Inventory].[Item] SET [CurrentQuantity] = @Quantity, [PurchaseDate] = @PurchaseDate WHERE [ItemID] = @ItemID"
                : "UPDATE [Inventory].[Item] SET [CurrentQuantity] = @Quantity WHERE [ItemID] = @ItemID";
            await connection.ExecuteAsync(sql, new { ItemID = itemId, Quantity = quantity, PurchaseDate = purchaseDate });
        }

        public async Task SetFavoriteAsync(int itemId, bool isFavorite)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "UPDATE [Inventory].[Item] SET [IsFavorite] = @IsFavorite WHERE [ItemID] = @ItemID";
            await connection.ExecuteAsync(sql, new { ItemID = itemId, IsFavorite = isFavorite });
        }
    }
}
