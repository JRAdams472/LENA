using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.RecipeSteps.Queries
{
    public record GetRecipeStepsByRecipeIdQuery(int RecipeId) : IRequest<IReadOnlyList<RecipeStep>>;

    public class GetRecipeStepsByRecipeIdQueryHandler : IRequestHandler<GetRecipeStepsByRecipeIdQuery, IReadOnlyList<RecipeStep>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeStepsByRecipeIdQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<IReadOnlyList<RecipeStep>> Handle(GetRecipeStepsByRecipeIdQuery request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.GetStepsByRecipeIdAsync(request.RecipeId, cancellationToken);
        }
    }
}
