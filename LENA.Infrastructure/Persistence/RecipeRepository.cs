using Dapper;

using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Recipe;

namespace LENA.Infrastructure.Persistence
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        private readonly ICurrentUserService _currentUser;
        private readonly TimeProvider _timeProvider;

        public RecipeRepository(IDbConnectionFactory connectionFactory, ICurrentUserService currentUser, TimeProvider? timeProvider = null) : base(connectionFactory)
        {
            _currentUser = currentUser;
            _timeProvider = timeProvider ?? TimeProvider.System;
        }

        public override async Task<Recipe> CreateAsync(Recipe entity, CancellationToken cancellationToken = default)
        {
            entity.RecipeID = await QuerySingleAsync<int>("[Recipe].[usp_Recipe_Create]", ToParameters(entity, false), cancellationToken);

            await SetFavoriteAsync(entity.RecipeID, entity.IsFavorite, cancellationToken);

            return entity;
        }

        public override async Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Recipe>("[Recipe].[usp_Recipe_GetById]", new { RecipeID = id, UserID = _currentUser.UserID }, cancellationToken);

        public async Task<Recipe?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Recipe>("[Recipe].[usp_Recipe_GetByName]", new { RecipeName = name, UserID = _currentUser.UserID }, cancellationToken);

        public override async Task<IReadOnlyList<Recipe>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Recipe>("[Recipe].[usp_Recipe_ListAll]", new { UserID = _currentUser.UserID }, cancellationToken);

        public async Task<LENA.Application.Models.PagedResult<Recipe>> ListPagedAsync(int pageNumber, int pageSize, string? search = null, bool isFavorite = false, CancellationToken ct = default)
            => await QueryPagedListAsync<Recipe>("[Recipe].[usp_Recipe_ListAllPaged]", pageNumber, pageSize, new { UserID = _currentUser.UserID, Search = search, IsFavorite = isFavorite }, ct);

        public override async Task<Recipe> UpdateAsync(Recipe entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Recipe].[usp_Recipe_Update]", ToParameters(entity, true), nameof(Recipe), entity.RecipeID, cancellationToken);

            await SetFavoriteAsync(entity.RecipeID, entity.IsFavorite, cancellationToken);

            return entity;
        }

        private DynamicParameters ToParameters(Recipe entity, bool forUpdate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RecipeName", entity.RecipeName);
            parameters.Add("Description", entity.Description);
            parameters.Add("Servings", entity.Servings);
            parameters.Add("PrepTimeMinutes", entity.PrepTimeMinutes);
            parameters.Add("CookTimeMinutes", entity.CookTimeMinutes);
            parameters.Add("IsActive", entity.IsActive);

            if (forUpdate)
            {
                parameters.Add("RecipeID", entity.RecipeID);
                parameters.Add("LastUpdatedBy", entity.LastUpdatedBy);
                parameters.Add("LastUpdatedDate", entity.LastUpdatedDate);
            }
            else
            {
                parameters.Add("CreatedBy", entity.CreatedBy);
                parameters.Add("CreateDate", entity.CreateDate);
            }

            return parameters;
        }

        public override async Task<Recipe> DeleteAsync(Recipe entity, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Recipe].[usp_Recipe_Delete]", new { entity.RecipeID }, nameof(Recipe), entity.RecipeID, cancellationToken);
            return entity;
        }

        public async Task SetFavoriteAsync(int recipeId, bool isFavorite, CancellationToken cancellationToken = default)
        {
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            await ExecuteCommandAsync("[Recipe].[usp_UserRecipePreference_SetFavorite]", new
            {
                UserID = _currentUser.UserID,
                RecipeID = recipeId,
                IsFavorite = isFavorite,
                CreatedBy = _currentUser.UserName,
                CreateDate = now,
                LastUpdatedBy = _currentUser.UserName,
                LastUpdatedDate = now
            }, cancellationToken);
        }

        public async Task<IReadOnlyList<RecipeItem>> GetItemsByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
            => await QueryListAsync<RecipeItem>("[Recipe].[usp_RecipeItem_GetByRecipeId]", new { RecipeID = recipeId }, cancellationToken);

        public async Task<IReadOnlyList<RecipeStep>> GetStepsByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
            => await QueryListAsync<RecipeStep>("[Recipe].[usp_RecipeStep_GetByRecipeId]", new { RecipeID = recipeId }, cancellationToken);

        public async Task<RecipeItem> AddOrUpdateRecipeItemAsync(RecipeItem recipeItem, CancellationToken cancellationToken = default)
            => await QuerySingleAsync<RecipeItem>("[Recipe].[usp_RecipeItem_Create]", new
            {
                recipeItem.RecipeID,
                recipeItem.ItemID,
                recipeItem.Quantity,
                recipeItem.UnitOfMeasure,
                recipeItem.Notes,
                recipeItem.IsOptional
            }, cancellationToken);

        public async Task RemoveRecipeItemAsync(int recipeId, int itemId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync("[Recipe].[usp_RecipeItem_Delete]", new { RecipeID = recipeId, ItemID = itemId }, nameof(RecipeItem), $"{recipeId}-{itemId}", cancellationToken);

        public async Task<RecipeStep> AddStepAsync(RecipeStep recipeStep, CancellationToken cancellationToken = default)
        {
            recipeStep.RecipeStepID = await QuerySingleAsync<int>("[Recipe].[usp_RecipeStep_Create]", new
            {
                recipeStep.RecipeID,
                recipeStep.StepNumber,
                recipeStep.Instruction,
                recipeStep.CreatedBy,
                recipeStep.CreateDate
            }, cancellationToken);
            return recipeStep;
        }

        public async Task<RecipeStep> UpdateStepAsync(RecipeStep recipeStep, CancellationToken cancellationToken = default)
        {
            await ExecuteRequiringMatchAsync("[Recipe].[usp_RecipeStep_Update]", new
            {
                recipeStep.RecipeStepID,
                recipeStep.RecipeID,
                recipeStep.StepNumber,
                recipeStep.Instruction,
                recipeStep.LastUpdatedBy,
                recipeStep.LastUpdatedDate
            }, nameof(RecipeStep), recipeStep.RecipeStepID, cancellationToken);
            return recipeStep;
        }

        public async Task DeleteStepAsync(int recipeStepId, int recipeId, CancellationToken cancellationToken = default)
            => await ExecuteRequiringMatchAsync(
                "[Recipe].[usp_RecipeStep_Delete]",
                new { RecipeStepID = recipeStepId, RecipeID = recipeId },
                nameof(RecipeStep),
                recipeStepId,
                cancellationToken);
    }
}