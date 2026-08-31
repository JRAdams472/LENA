using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.RecipeItems.Commands
{
    public record AddOrUpdateRecipeItemCommand(RecipeItem RecipeItem) : IRequest<RecipeItem>;

    public class AddOrUpdateRecipeItemCommandHandler : IRequestHandler<AddOrUpdateRecipeItemCommand, RecipeItem>
    {
        private readonly IRecipeRepository _recipeRepository;

        public AddOrUpdateRecipeItemCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeItem> Handle(AddOrUpdateRecipeItemCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.AddOrUpdateRecipeItemAsync(request.RecipeItem, cancellationToken);
        }
    }
}
