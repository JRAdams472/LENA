using LENA.Application.Models;
using LENA.Domain.Entity.Recipe;

namespace LENA.Application.Contracts.Persistence
{
    /// <summary>
    /// Recipes are shared across users. Reads are scoped by UserID so each user sees their own favorite status.
    /// Writes (create/update/delete) are not user-scoped because the recipe catalog is global; only favorites are per-user.
    /// </summary>
    public interface IRecipeRepository : IAsyncRepository<Recipe>, IGetByNameRepository<Recipe>
    {
        Task<IReadOnlyList<RecipeItem>> GetItemsByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RecipeStep>> GetStepsByRecipeIdAsync(int recipeId, CancellationToken cancellationToken = default);
        Task<RecipeItem> AddOrUpdateRecipeItemAsync(RecipeItem recipeItem, CancellationToken cancellationToken = default);
        Task RemoveRecipeItemAsync(int recipeId, int itemId, CancellationToken cancellationToken = default);
        Task<RecipeStep> AddStepAsync(RecipeStep recipeStep, CancellationToken cancellationToken = default);
        Task<RecipeStep> UpdateStepAsync(RecipeStep recipeStep, CancellationToken cancellationToken = default);
        Task DeleteStepAsync(int recipeStepId, int recipeId, CancellationToken cancellationToken = default);
        Task SetFavoriteAsync(int recipeId, bool isFavorite, CancellationToken cancellationToken = default);

        Task<PagedResult<Recipe>> ListPagedAsync(int pageNumber, int pageSize, string? search = null, bool isFavorite = false, CancellationToken ct = default);
    }
}