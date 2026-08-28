using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IItemRepository : IAsyncRepository<Item>
    {
        // Change the category of an existing item
        Task ChangeItemCategoryAsync(int itemId, int newCategoryId);

        // Add or update only the barcode for an existing item
        Task AddOrUpdateBarCodeAsync(int itemId, string barcode);

        // Change the quantity of an existing item (with optional purchase date)
        Task AdjustQuantityAsync(int itemId, decimal quantity, DateTime? purchaseDate = null);

        // Set the favorite flag of an item
        Task SetFavoriteAsync(int itemId, bool isFavorite);
    }
}
