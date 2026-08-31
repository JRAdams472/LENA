using LENA.Application.Contracts.Persistence;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.Recipes.Commands
{
    public record DeleteRecipeCommand(int RecipeId) : IRequest<RecipeEntity?>;

    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, RecipeEntity?>
    {
        private readonly IRecipeRepository _recipeRepository;

        public DeleteRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeEntity?> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.RecipeId, cancellationToken);
            if (recipe == null)
                return null;

            return await _recipeRepository.DeleteAsync(recipe, cancellationToken);
        }
    }
}
