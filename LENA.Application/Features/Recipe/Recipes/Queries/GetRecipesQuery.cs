using LENA.Application.Contracts.Persistence;

using MediatR;

using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;

namespace LENA.Application.Features.Recipe.Recipes.Queries
{
    public record GetRecipesQuery : IRequest<IReadOnlyList<RecipeEntity>>;

    public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, IReadOnlyList<RecipeEntity>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<IReadOnlyList<RecipeEntity>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.ListAllAsync(cancellationToken);
        }
    }
}