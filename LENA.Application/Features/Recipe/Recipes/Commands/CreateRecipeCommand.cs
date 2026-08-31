using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.Recipes.Commands
{
    public record CreateRecipeCommand(RecipeEntity Recipe) : IRequest<RecipeEntity>, ICreateCommand
    {
        public AuditableEntity AuditableEntity => Recipe;
    }

    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeEntity>
    {
        private readonly IRecipeRepository _recipeRepository;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeEntity> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.CreateAsync(request.Recipe, cancellationToken);
        }
    }
}
