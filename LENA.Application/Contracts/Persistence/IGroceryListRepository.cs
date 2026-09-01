using LENA.Domain.Entity.Grocery;

namespace LENA.Application.Contracts.Persistence
{
    public interface IGroceryListRepository : IAsyncRepository<GroceryList>
    {
        Task<GroceryList> GenerateFromMealPlanAsync(GroceryList groceryList, CancellationToken cancellationToken = default);
        Task<GroceryListItem> AddGroceryListItemAsync(GroceryListItem groceryListItem, CancellationToken cancellationToken = default);
        Task<GroceryListItem> ToggleGroceryListItemCheckedAsync(GroceryListItem groceryListItem, CancellationToken cancellationToken = default);
        Task DeleteGroceryListItemAsync(int groceryListItemId, CancellationToken cancellationToken = default);
    }
}
