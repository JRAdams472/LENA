using LENA.Application.Contracts.Persistence;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.Recipes.Commands
{
    public record UpdateRecipeCommand(RecipeEntity Recipe) : IRequest<RecipeEntity>;

    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeEntity>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeEntity> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.UpdateAsync(request.Recipe, cancellationToken);
        }
    }
}
