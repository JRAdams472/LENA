using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;

using MediatR;

using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;
using RecipeItem = LENA.Domain.Entity.Recipe.RecipeItem;
using RecipeStep = LENA.Domain.Entity.Recipe.RecipeStep;

namespace LENA.Application.Features.Recipe.Recipes.Queries
{
    public record GetRecipeByIdQuery(int RecipeId) : IRequest<RecipeEntity?>;

    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeEntity?>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeEntity?> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId, cancellationToken) ?? throw new NotFoundException(nameof(RecipeEntity), request.RecipeId);

            var items = await _recipeRepository.GetItemsByRecipeIdAsync(request.RecipeId, cancellationToken);
            var steps = await _recipeRepository.GetStepsByRecipeIdAsync(request.RecipeId, cancellationToken);

            recipe.RecipeItems = new List<RecipeItem>(items);
            recipe.RecipeSteps = new List<RecipeStep>(steps);

            return recipe;
        }
    }
}