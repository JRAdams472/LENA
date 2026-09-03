using FluentValidation;

using LENA.Application.Features.Recipe.RecipeItems.Commands;
using LENA.Domain.Entity.Recipe;

namespace LENA.Application.Features.Recipe.RecipeItems.Validators
{
    public class AddOrUpdateRecipeItemCommandValidator : AbstractValidator<AddOrUpdateRecipeItemCommand>
    {
        public AddOrUpdateRecipeItemCommandValidator()
        {
            RuleFor(x => x.RecipeItem).NotNull().WithMessage("Recipe item is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RecipeItem.RecipeID).GreaterThan(0).WithMessage("Recipe ID is required");
                    RuleFor(x => x.RecipeItem.ItemID).GreaterThan(0).WithMessage("Item ID is required");
                    RuleFor(x => x.RecipeItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
                });
        }
    }
}