using LENA.Application.Contracts.Auditing;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Common;
using LENA.Domain.Entity.Recipe;
using MediatR;

namespace LENA.Application.Features.Recipe.RecipeSteps.Commands
{
    public record UpdateRecipeStepCommand(RecipeStep RecipeStep) : IRequest<RecipeStep>, IUpdateCommand
    {
        public AuditableEntity AuditableEntity => RecipeStep;
    }

    public class UpdateRecipeStepCommandHandler : IRequestHandler<UpdateRecipeStepCommand, RecipeStep>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeStepCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<RecipeStep> Handle(UpdateRecipeStepCommand request, CancellationToken cancellationToken)
        {
            return await _recipeRepository.UpdateStepAsync(request.RecipeStep, cancellationToken);
        }
    }
}
