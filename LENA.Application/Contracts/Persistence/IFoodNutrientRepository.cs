using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFoodNutrientRepository : IAsyncRepository<FoodNutrient>
    {
        Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId);
        Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId);
        Task<FoodNutrient?> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId);
    }
}