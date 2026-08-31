using FluentValidation;
using LENA.Application.Features.Recipe.Recipes.Commands;

namespace LENA.Application.Features.Recipe.Recipes.Validators
{
    public class DeleteRecipeCommandValidator : AbstractValidator<DeleteRecipeCommand>
    {
        public DeleteRecipeCommandValidator()
        {
            RuleFor(x => x.RecipeId).GreaterThan(0).WithMessage("Recipe ID is required");
        }
    }
}
