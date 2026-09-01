using LENA.Application.Contracts.Persistence;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.Recipes.Queries
{
    public record GetRecipesQuery(LENA.Application.Models.PaginationRequest? Paging = null) : IRequest<LENA.Application.Models.PagedResult<RecipeEntity>>;

    public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, LENA.Application.Models.PagedResult<RecipeEntity>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<LENA.Application.Models.PagedResult<RecipeEntity>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.ListAllAsync(request.Paging, cancellationToken);
        }
    }
}
