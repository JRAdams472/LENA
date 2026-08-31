using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.RecipeSteps.Commands
{
    public record AddRecipeStepCommand(RecipeStep RecipeStep) : IRequest<RecipeStep>;

    public class AddRecipeStepCommandHandler : IRequestHandler<AddRecipeStepCommand, RecipeStep>
    {
        private readonly IRecipeRepository _recipeRepository;

        public AddRecipeStepCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeStep> Handle(AddRecipeStepCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.AddStepAsync(request.RecipeStep, cancellationToken);
        }
    }
}
