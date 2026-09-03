using LENA.Domain.Entity.Inventory;

namespace LENA.Application.Contracts.Persistence
{
    public interface IFoodNutrientRepository : IAsyncRepository<FoodNutrient>
    {
        Task<IEnumerable<FoodNutrient>> GetByFoodIdAsync(int foodId, CancellationToken cancellationToken = default);
        Task<IEnumerable<FoodNutrient>> GetByNutrientIdAsync(int nutrientId, CancellationToken cancellationToken = default);
        Task<FoodNutrient?> GetByFoodAndNutrientIdAsync(int foodId, int nutrientId, CancellationToken cancellationToken = default);
    }
}