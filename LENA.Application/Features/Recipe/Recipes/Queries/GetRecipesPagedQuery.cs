using LENA.Application.Contracts.Persistence;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.Recipes.Queries
{
    public record GetRecipesPagedQuery(int PageNumber, int PageSize, string? Search, bool IsFavorite) : IRequest<LENA.Application.Models.PagedResult<RecipeEntity>>;

    public class GetRecipesPagedQueryHandler : IRequestHandler<GetRecipesPagedQuery, LENA.Application.Models.PagedResult<RecipeEntity>>
    {
        private readonly IRecipeRepository _recipeRepository;
        public GetRecipesPagedQueryHandler(IRecipeRepository recipeRepository) => _recipeRepository = recipeRepository;
        public async Task<LENA.Application.Models.PagedResult<RecipeEntity>> Handle(GetRecipesPagedQuery request, CancellationToken cancellationToken)
            => await _recipeRepository.ListPagedAsync(request.PageNumber, request.PageSize, request.Search, request.IsFavorite, cancellationToken);
    }
}
