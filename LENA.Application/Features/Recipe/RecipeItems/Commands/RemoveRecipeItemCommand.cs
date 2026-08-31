using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Recipe.RecipeItems.Commands
{
    public record RemoveRecipeItemCommand(int RecipeId, int ItemId) : IRequest<Unit>;

    public class RemoveRecipeItemCommandHandler : IRequestHandler<RemoveRecipeItemCommand, Unit>
    {
        private readonly IRecipeRepository _recipeRepository;

        public RemoveRecipeItemCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Unit> Handle(RemoveRecipeItemCommand request, CancellationToken cancellationToken)
        {
            await _recipeRepository.RemoveRecipeItemAsync(request.RecipeId, request.ItemId, cancellationToken);
            return Unit.Value;
        }
    }
}
