using LENA.Application.Models;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IItemRepository : IAsyncRepository<Item>
    {
        // Change the category of an existing item
        Task ChangeItemCategoryAsync(int itemId, int newCategoryId, CancellationToken cancellationToken = default);

        // Add or update only the UPC 12 for an existing item
        Task AddOrUpdateUPC12Async(int itemId, string upc12, CancellationToken cancellationToken = default);

        // Add or update only the UPC 14 for an existing item
        Task AddOrUpdateUPC14Async(int itemId, string upc14, CancellationToken cancellationToken = default);

        // Change the quantity of an existing item (with optional purchase date)
        Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null, string? lastUpdatedBy = null, CancellationToken cancellationToken = default);

        // Set the favorite flag of an item
        Task SetFavoriteAsync(int itemId, bool isFavorite, CancellationToken cancellationToken = default);

        Task<PagedResult<Item>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
