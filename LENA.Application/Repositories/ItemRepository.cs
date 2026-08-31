using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        // This repository would typically use a database context or direct SQL queries
        // For now, it's using the base implementation

        public async Task ChangeItemCategoryAsync(int itemId, int newCategoryId)
        {
            // This would be implemented with actual database access
            // Get the item
            var item = await GetByIdAsync(itemId);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found.");
            }

            // Check if the new category exists (you would typically load it from DB)
            // For now, just update the category ID
            item.CategoryID = newCategoryId;
            
            // Save the updated item
            await UpdateAsync(item);
        }

        public async Task AddOrUpdateUPC12Async(int itemId, string upc12)
        {
            // This would be implemented with actual database access
            // Get the item
            var item = await GetByIdAsync(itemId);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found.");
            }

            item.UPC12 = upc12;
            
            // Save the updated item
            await UpdateAsync(item);
        }

        public async Task AddOrUpdateUPC14Async(int itemId, string upc14)
        {
            // This would be implemented with actual database access
            // Get the item
            var item = await GetByIdAsync(itemId);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found.");
            }

            item.UPC14 = upc14;
            
            // Save the updated item
            await UpdateAsync(item);
        }

        public async Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null)
        {
            // This would be implemented with actual database access
            // Get the item
            var item = await GetByIdAsync(itemId);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found.");
            }

            item.CurrentQuantity = quantity;
            
            // Save the updated item
            if (purchaseDate.HasValue)
            {
                item.PurchaseDate = purchaseDate.Value;
            }
            
            await UpdateAsync(item);
        }

        public async Task SetFavoriteAsync(int itemId, bool isFavorite)
        {
            // This would be implemented with actual database access
            // Get the item
            var item = await GetByIdAsync(itemId);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found.");
            }

            item.IsFavorite = isFavorite;
            
            // Save the updated item
            await UpdateAsync(item);
        }
    }
}