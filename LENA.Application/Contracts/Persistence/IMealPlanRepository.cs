using LENA.Domain.Entity.MealPlan;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Contracts.Persistence
{
    public interface IMealPlanRepository : IAsyncRepository<MealPlanEntity>
    {
        Task<IReadOnlyList<MealSlot>> GetSlotsByMealPlanIdAsync(int mealPlanId, CancellationToken cancellationToken = default);
        Task<MealSlot> AddSlotAsync(MealSlot mealSlot, CancellationToken cancellationToken = default);
        Task<MealSlot> UpdateSlotAsync(MealSlot mealSlot, CancellationToken cancellationToken = default);
        Task DeleteSlotAsync(int mealSlotId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MealSlotItem>> GetSlotItemsBySlotIdAsync(int mealSlotId, CancellationToken cancellationToken = default);
        Task<MealSlotItem> AddSlotItemAsync(MealSlotItem mealSlotItem, CancellationToken cancellationToken = default);
        Task DeleteSlotItemAsync(int mealSlotItemId, CancellationToken cancellationToken = default);
    }
}
