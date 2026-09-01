using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<IReadOnlyList<Item>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Item>("[Inventory].[usp_Item_ListAll]", cancellationToken: cancellationToken);

        public override async Task<Item?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Item>("[Inventory].[usp_Item_GetById]", new { Id = id }, cancellationToken);

        public override async Task<Item?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Item>("[Inventory].[usp_Item_GetByName]", new { Name = name }, cancellationToken);

        public override async Task<Item> CreateAsync(Item entity, CancellationToken cancellationToken = default)
        {
            entity.ItemID = await QuerySingleAsync<int>("[Inventory].[usp_Item_Create]", new
            {
                entity.Name,
                entity.Brand,
                entity.UPC12,
                entity.UPC14,
                entity.CategoryID,
                entity.Unit,
                entity.CurrentQuantity,
                entity.MinQuantity,
                entity.PurchaseDate,
                entity.ExpiryDate,
                entity.Notes,
                entity.IsFavorite,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<Item> UpdateAsync(Item entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_Item_Update]", new
            {
                entity.ItemID,
                entity.Name,
                entity.Brand,
                entity.UPC12,
                entity.UPC14,
                entity.CategoryID,
                entity.Unit,
                entity.CurrentQuantity,
                entity.MinQuantity,
                entity.PurchaseDate,
                entity.ExpiryDate,
                entity.Notes,
                entity.IsFavorite,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(Item), entity.ItemID, cancellationToken);
            return entity;
        }

        public override async Task<Item> DeleteAsync(Item entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Inventory].[usp_Item_Delete]", new { entity.ItemID }, nameof(Item), entity.ItemID, cancellationToken);
            return entity;
        }

        public async Task ChangeItemCategoryAsync(int itemId, int newCategoryId, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_ChangeItemCategory]", new { ItemID = itemId, CategoryID = newCategoryId }, cancellationToken);

        public async Task AddOrUpdateUPC12Async(int itemId, string upc12, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_AddOrUpdateUPC12]", new { ItemID = itemId, UPC12 = upc12 }, cancellationToken);

        public async Task AddOrUpdateUPC14Async(int itemId, string upc14, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_AddOrUpdateUPC14]", new { ItemID = itemId, UPC14 = upc14 }, cancellationToken);

        public async Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null, string? lastUpdatedBy = null, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_AdjustQuantity]", new { ItemID = itemId, Quantity = quantity, PurchaseDate = purchaseDate, LastUpdatedBy = lastUpdatedBy }, cancellationToken);

        public async Task SetFavoriteAsync(int itemId, bool isFavorite, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Inventory].[usp_Item_SetFavorite]", new { ItemID = itemId, IsFavorite = isFavorite }, cancellationToken);
    }
}
