using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFoodFlavorRepository : IAsyncRepository<FoodFlavor>
    {
        Task<IEnumerable<FoodFlavor>> GetByFoodIdAsync(int foodId);
        Task<IEnumerable<FoodFlavor>> GetByFlavorIdAsync(int flavorId);
        Task<FoodFlavor?> GetByFoodAndFlavorIdAsync(int foodId, int flavorId);
    }
}