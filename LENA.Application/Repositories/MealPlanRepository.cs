using System.Linq;
using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.MealPlan;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.Application.Repositories
{
    public class MealPlanRepository : BaseRepository<MealPlanEntity>, IMealPlanRepository
    {
        private readonly ICurrentUserService _currentUser;

        public MealPlanRepository(IDbConnectionFactory connectionFactory, ICurrentUserService currentUser) : base(connectionFactory)
        {
            _currentUser = currentUser;
        }

        public override async Task<MealPlanEntity> CreateAsync(MealPlanEntity entity, CancellationToken cancellationToken = default)
        {
            entity.MealPlanID = await QuerySingleAsync<int>("[MealPlan].[usp_MealPlan_Create]", new
            {
                entity.PlanName,
                entity.WeekStartDate,
                entity.WeekStartDayOfWeek,
                entity.IsActive,
                UserID = _currentUser.UserID,
                entity.CreatedBy,
                entity.CreateDate
            }, cancellationToken);
            return entity;
        }

        public override async Task<MealPlanEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_GetById]", new { MealPlanID = id, UserID = _currentUser.UserID }, cancellationToken);

        public async Task<MealPlanEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_GetByName]", new { PlanName = name, UserID = _currentUser.UserID }, cancellationToken);

        public override async Task<IReadOnlyList<MealPlanEntity>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_ListAll]", new { UserID = _currentUser.UserID }, cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<MealPlanEntity>> ListPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
            => await QueryPagedListAsync<MealPlanEntity>("[MealPlan].[usp_MealPlan_ListAllPaged]", pageNumber, pageSize, new { UserID = _currentUser.UserID }, ct);

        public async Task<IReadOnlyList<MealPlanNutritionRow>> GetMealPlanNutritionAsync(int mealPlanId, CancellationToken cancellationToken = default)
            => await QueryListAsync<MealPlanNutritionRow>("[MealPlan].[usp_MealPlan_GetNutrition]", new { MealPlanID = mealPlanId, UserID = _currentUser.UserID }, cancellationToken);

        public override async Task<MealPlanEntity> UpdateAsync(MealPlanEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealPlan_Update]", new
            {
                entity.MealPlanID,
                entity.PlanName,
                entity.WeekStartDate,
                entity.WeekStartDayOfWeek,
                entity.IsActive,
                UserID = _currentUser.UserID,
                entity.LastUpdatedBy,
                entity.LastUpdatedDate
            }, nameof(MealPlanEntity), entity.MealPlanID, cancellationToken);
            return entity;
        }

        public override async Task<MealPlanEntity> DeleteAsync(MealPlanEntity entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealPlan_Delete]", new { entity.MealPlanID, UserID = _currentUser.UserID }, nameof(MealPlanEntity), entity.MealPlanID, cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<MealSlot>> GetSlotsByMealPlanIdAsync(int mealPlanId, CancellationToken cancellationToken = default)
            => await QueryListAsync<MealSlot>("[MealPlan].[usp_MealSlot_GetByMealPlanId]", new { MealPlanID = mealPlanId, UserID = _currentUser.UserID }, cancellationToken);

        public async Task<MealSlot> AddSlotAsync(MealSlot mealSlot, CancellationToken cancellationToken = default)
        {
            mealSlot.MealSlotID = await QuerySingleAsync<int>("[MealPlan].[usp_MealSlot_Create]", new
            {
                mealSlot.MealPlanID,
                mealSlot.DayOfWeek,
                mealSlot.MealType,
                mealSlot.RecipeID,
                mealSlot.Servings,
                mealSlot.ReplacementNote,
                UserID = _currentUser.UserID,
                mealSlot.CreatedBy,
                mealSlot.CreateDate
            }, cancellationToken);

            if (mealSlot.MealSlotID == 0)
                throw new NotFoundException(nameof(MealPlanEntity), mealSlot.MealPlanID);

            return mealSlot;
        }

        public async Task<MealSlot> UpdateSlotAsync(MealSlot mealSlot, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealSlot_Update]", new
            {
                mealSlot.MealPlanID,
                mealSlot.DayOfWeek,
                mealSlot.MealType,
                mealSlot.RecipeID,
                mealSlot.Servings,
                mealSlot.ReplacementNote,
                UserID = _currentUser.UserID,
                mealSlot.LastUpdatedBy,
                mealSlot.LastUpdatedDate,
                mealSlot.MealSlotID
            }, nameof(MealSlot), mealSlot.MealSlotID, cancellationToken);
            return mealSlot;
        }

        public async Task DeleteSlotAsync(int mealSlotId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealSlot_Delete]", new { MealSlotID = mealSlotId, UserID = _currentUser.UserID }, nameof(MealSlot), mealSlotId, cancellationToken);

        public async Task<IReadOnlyList<MealSlotItem>> GetSlotItemsBySlotIdAsync(int mealSlotId, CancellationToken cancellationToken = default)
            => await QueryListAsync<MealSlotItem>("[MealPlan].[usp_MealSlotItem_GetBySlotId]", new { MealSlotID = mealSlotId, UserID = _currentUser.UserID }, cancellationToken);

        public async Task<MealSlotItem> AddSlotItemAsync(MealSlotItem mealSlotItem, CancellationToken cancellationToken = default)
        {
            mealSlotItem.MealSlotItemID = await QuerySingleAsync<int>("[MealPlan].[usp_MealSlotItem_Create]", new
            {
                mealSlotItem.MealSlotID,
                mealSlotItem.ItemID,
                mealSlotItem.Quantity,
                mealSlotItem.UnitOfMeasure,
                mealSlotItem.IsFromRecipe,
                UserID = _currentUser.UserID,
                mealSlotItem.CreatedBy,
                mealSlotItem.CreateDate
            }, cancellationToken);

            if (mealSlotItem.MealSlotItemID == 0)
                throw new NotFoundException(nameof(MealSlot), mealSlotItem.MealSlotID);

            return mealSlotItem;
        }

        public async Task DeleteSlotItemAsync(int mealSlotItemId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[MealPlan].[usp_MealSlotItem_Delete]", new { MealSlotItemID = mealSlotItemId, UserID = _currentUser.UserID }, nameof(MealSlotItem), mealSlotItemId, cancellationToken);
    }
}
