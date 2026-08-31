using System.Linq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.MealPlan;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Repositories
{
    public class MealPlanRepository : BaseRepository<MealPlanEntity>, IMealPlanRepository
    {
        public MealPlanRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<MealPlanEntity> CreateAsync(MealPlanEntity entity, CancellationToken cancellationToken = default)
        {
            entity.MealPlanID = await QuerySingleAsync<int>("[MealPlan].[usp_MealPlan_Create]", new
            {
                entity.PlanName,
                entity.WeekStartDate,
                entity.WeekStartDayOfWeek,
                entity.IsActive,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<MealPlanEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_GetById]", new { MealPlanID = id }, cancellationToken);

        public override async Task<MealPlanEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var all = await QueryListAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_ListAll]", cancellationToken: cancellationToken);
            return all.FirstOrDefault(x => x.PlanName == name);
        }

        public override async Task<IReadOnlyList<MealPlanEntity>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_ListAll]", cancellationToken: cancellationToken);

        public async Task<IReadOnlyList<MealPlanNutritionRow>> GetMealPlanNutritionAsync(int mealPlanId, CancellationToken cancellationToken = default)
            => await QueryListAsync<MealPlanNutritionRow>("[MealPlan].[usp_MealPlan_GetNutrition]", new { MealPlanID = mealPlanId }, cancellationToken);

        public override async Task<MealPlanEntity> UpdateAsync(MealPlanEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealPlan_Update]", new
            {
                entity.MealPlanID,
                entity.PlanName,
                entity.WeekStartDate,
                entity.WeekStartDayOfWeek,
                entity.IsActive,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(MealPlanEntity), entity.MealPlanID, cancellationToken);
            return entity;
        }

        public override async Task<MealPlanEntity> DeleteAsync(MealPlanEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealPlan_Delete]", new { entity.MealPlanID }, nameof(MealPlanEntity), entity.MealPlanID, cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<MealSlot>> GetSlotsByMealPlanIdAsync(int mealPlanId, CancellationToken cancellationToken = default)
            => await QueryListAsync<MealSlot>("[MealPlan].[usp_MealSlot_GetByMealPlanId]", new { MealPlanID = mealPlanId }, cancellationToken);

        public async Task<MealSlot> AddSlotAsync(MealSlot mealSlot, CancellationToken cancellationToken = default)
        {
            mealSlot.MealSlotID = await QuerySingleAsync<int>("[MealPlan].[usp_MealSlot_Create]", new
            {
                mealSlot.MealPlanID,
                mealSlot.DayOfWeek,
                mealSlot.MealType,
                mealSlot.RecipeID,
                mealSlot.ReplacementNote,
                mealSlot.CreatedBy,
                mealSlot.CreateDate
            }, cancellationToken);
            return mealSlot;
        }

        public async Task<MealSlot> UpdateSlotAsync(MealSlot mealSlot, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealSlot_Update]", new
            {
                mealSlot.MealSlotID,
                mealSlot.MealPlanID,
                mealSlot.DayOfWeek,
                mealSlot.MealType,
                mealSlot.RecipeID,
                mealSlot.ReplacementNote,
                mealSlot.LastUpdatedBy,
                mealSlot.LastUpdatedDate
            }, nameof(MealSlot), mealSlot.MealSlotID, cancellationToken);
            return mealSlot;
        }

        public async Task DeleteSlotAsync(int mealSlotId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealSlot_Delete]", new { MealSlotID = mealSlotId }, nameof(MealSlot), mealSlotId, cancellationToken);

        public async Task<IReadOnlyList<MealSlotItem>> GetSlotItemsBySlotIdAsync(int mealSlotId, CancellationToken cancellationToken = default)
            => await QueryListAsync<MealSlotItem>("[MealPlan].[usp_MealSlotItem_GetBySlotId]", new { MealSlotID = mealSlotId }, cancellationToken);

        public async Task<MealSlotItem> AddSlotItemAsync(MealSlotItem mealSlotItem, CancellationToken cancellationToken = default)
        {
            mealSlotItem.MealSlotItemID = await QuerySingleAsync<int>("[MealPlan].[usp_MealSlotItem_Create]", new
            {
                mealSlotItem.MealSlotID,
                mealSlotItem.ItemID,
                mealSlotItem.Quantity,
                mealSlotItem.UnitOfMeasure,
                mealSlotItem.IsFromRecipe,
                mealSlotItem.CreatedBy,
                mealSlotItem.CreateDate
            }, cancellationToken);
            return mealSlotItem;
        }

        public async Task DeleteSlotItemAsync(int mealSlotItemId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealSlotItem_Delete]", new { MealSlotItemID = mealSlotItemId }, nameof(MealSlotItem), mealSlotItemId, cancellationToken);
    }
}
