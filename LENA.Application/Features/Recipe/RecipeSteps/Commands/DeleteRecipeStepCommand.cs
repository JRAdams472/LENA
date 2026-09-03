using LENA.Application.Contracts.Persistence;

using MediatR;

namespace LENA.Application.Features.Recipe.RecipeSteps.Commands
{
    public record DeleteRecipeStepCommand(int RecipeStepId, int RecipeId) : IRequest<Unit>;

    public class DeleteRecipeStepCommandHandler : IRequestHandler<DeleteRecipeStepCommand, Unit>
    {
        private readonly IRecipeRepository _recipeRepository;

        public DeleteRecipeStepCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Unit> Handle(DeleteRecipeStepCommand request, CancellationToken cancellationToken)
        {
            await _recipeRepository.DeleteStepAsync(request.RecipeStepId, request.RecipeId, cancellationToken);
            return Unit.Value;
        }
    }
}