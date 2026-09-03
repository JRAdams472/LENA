using FluentValidation;

using LENA.Application.Features.Recipe.Recipes.Commands;
using LENA.Domain.Entity.Recipe;

namespace LENA.Application.Features.Recipe.Recipes.Validators
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(x => x.Recipe).NotNull().WithMessage("Recipe is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Recipe.RecipeName).NotEmpty().WithMessage("Recipe name is required");
                    RuleFor(x => x.Recipe.Servings).NotNull().GreaterThan(0).WithMessage("Servings must be greater than 0");
                });
        }
    }
}