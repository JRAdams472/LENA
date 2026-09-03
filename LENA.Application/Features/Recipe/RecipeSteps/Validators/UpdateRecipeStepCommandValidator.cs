using FluentValidation;

using LENA.Application.Features.Recipe.RecipeSteps.Commands;
using LENA.Domain.Entity.Recipe;

namespace LENA.Application.Features.Recipe.RecipeSteps.Validators
{
    public class UpdateRecipeStepCommandValidator : AbstractValidator<UpdateRecipeStepCommand>
    {
        public UpdateRecipeStepCommandValidator()
        {
            RuleFor(x => x.RecipeStep).NotNull().WithMessage("Recipe step is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RecipeStep.RecipeStepID).GreaterThan(0).WithMessage("Recipe step ID is required");
                    RuleFor(x => x.RecipeStep.RecipeID).GreaterThan(0).WithMessage("Recipe ID is required");
                    RuleFor(x => x.RecipeStep.StepNumber).GreaterThan(0).WithMessage("Step number must be greater than 0");
                    RuleFor(x => x.RecipeStep.Instruction).NotEmpty().WithMessage("Instruction is required");
                });
        }
    }
}