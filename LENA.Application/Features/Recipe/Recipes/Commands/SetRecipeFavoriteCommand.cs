using LENA.Application.Contracts.Persistence;
using MediatR;

namespace LENA.Application.Features.Recipe.Recipes.Commands
{
    public record SetRecipeFavoriteCommand(int RecipeId, bool IsFavorite) : IRequest<Unit>;

    public class SetRecipeFavoriteCommandHandler : IRequestHandler<SetRecipeFavoriteCommand, Unit>
    {
        private readonly IRecipeRepository _recipeRepository;

        public SetRecipeFavoriteCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Unit> Handle(SetRecipeFavoriteCommand request, CancellationToken cancellationToken)
        {
            await _recipeRepository.SetFavoriteAsync(request.RecipeId, request.IsFavorite, cancellationToken);
            return Unit.Value;
        }
    }
}
