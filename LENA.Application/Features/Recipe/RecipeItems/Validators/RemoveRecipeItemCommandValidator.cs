using FluentValidation;

using LENA.Application.Features.Recipe.RecipeItems.Commands;

namespace LENA.Application.Features.Recipe.RecipeItems.Validators
{
    public class RemoveRecipeItemCommandValidator : AbstractValidator<RemoveRecipeItemCommand>
    {
        public RemoveRecipeItemCommandValidator()
        {
            RuleFor(x => x.RecipeId).GreaterThan(0).WithMessage("Recipe ID is required");
            RuleFor(x => x.ItemId).GreaterThan(0).WithMessage("Item ID is required");
        }
    }
}