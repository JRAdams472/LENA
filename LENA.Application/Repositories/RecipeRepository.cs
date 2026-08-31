using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Recipe;

namespace LENA.Application.Repositories
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public override async Task<Recipe> CreateAsync(Recipe entity, CancellationToken cancellationToken = default)
        {
            entity.RecipeID = await QuerySingleAsync<int>("[Recipe].[usp_Recipe_Create]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Recipe>("[Recipe].[usp_Recipe_GetById]", new { RecipeID = id }, cancellationToken);

        public override async Task<Recipe?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => await QueryFirstAsync<Recipe>("[Recipe].[usp_Recipe_GetByName]", new { RecipeName = name }, cancellationToken);

        public override async Task<IReadOnlyList<Recipe>> ListAllAsync(CancellationToken cancellationToken = default)
            => await QueryListAsync<Recipe>("[Recipe].[usp_Recipe_ListAll]", cancellationToken: cancellationToken);

        public override async Task<Recipe> UpdateAsync(Recipe entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Recipe].[usp_Recipe_Update]", entity, cancellationToken);
            return entity;
        }

        public override async Task<Recipe> DeleteAsync(Recipe entity, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Recipe].[usp_Recipe_Delete]", new { RecipeID = entity.RecipeID }, cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<RecipeItem>> GetItemsByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
            => await QueryListAsync<RecipeItem>("[Recipe].[usp_RecipeItem_GetByRecipeId]", new { RecipeID = recipeId }, cancellationToken);

        public async Task<IReadOnlyList<RecipeStep>> GetStepsByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default)
            => await QueryListAsync<RecipeStep>("[Recipe].[usp_RecipeStep_GetByRecipeId]", new { RecipeID = recipeId }, cancellationToken);

        public async Task<RecipeItem> AddOrUpdateRecipeItemAsync(RecipeItem recipeItem, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Recipe].[usp_RecipeItem_Create]", recipeItem, cancellationToken);
            return recipeItem;
        }

        public async Task RemoveRecipeItemAsync(int recipeId, int itemId, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Recipe].[usp_RecipeItem_Delete]", new { RecipeID = recipeId, ItemID = itemId }, cancellationToken);

        public async Task<RecipeStep> AddStepAsync(RecipeStep recipeStep, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Recipe].[usp_RecipeStep_Create]", recipeStep, cancellationToken);
            return recipeStep;
        }

        public async Task<RecipeStep> UpdateStepAsync(RecipeStep recipeStep, CancellationToken cancellationToken = default)
        {
            await ExecuteCommandAsync("[Recipe].[usp_RecipeStep_Update]", recipeStep, cancellationToken);
            return recipeStep;
        }

        public async Task DeleteStepAsync(int recipeStepId, CancellationToken cancellationToken = default)
            => await ExecuteCommandAsync("[Recipe].[usp_RecipeStep_Delete]", new { RecipeStepID = recipeStepId }, cancellationToken);
    }
}
