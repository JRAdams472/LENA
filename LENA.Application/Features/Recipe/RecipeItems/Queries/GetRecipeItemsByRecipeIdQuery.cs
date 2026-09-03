using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Recipe;

using MediatR;

namespace LENA.Application.Features.Recipe.RecipeItems.Queries
{
    public record GetRecipeItemsByRecipeIdQuery(int RecipeId) : IRequest<IReadOnlyList<RecipeItem>>;

    public class GetRecipeItemsByRecipeIdQueryHandler : IRequestHandler<GetRecipeItemsByRecipeIdQuery, IReadOnlyList<RecipeItem>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeItemsByRecipeIdQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<IReadOnlyList<RecipeItem>> Handle(GetRecipeItemsByRecipeIdQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.GetItemsByRecipeIdAsync(request.RecipeId, cancellationToken);
        }
    }
}