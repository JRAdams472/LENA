using FluentValidation;

using LENA.Application.Features.Recipe.RecipeSteps.Commands;

namespace LENA.Application.Features.Recipe.RecipeSteps.Validators
{
    public class DeleteRecipeStepCommandValidator : AbstractValidator<DeleteRecipeStepCommand>
    {
        public DeleteRecipeStepCommandValidator()
        {
            RuleFor(x => x.RecipeStepId).GreaterThan(0).WithMessage("Recipe step ID is required");
            RuleFor(x => x.RecipeId).GreaterThan(0).WithMessage("Recipe ID is required");
        }
    }
}