using FluentValidation;
using LENA.Application.Features.Grocery.GroceryLists.Commands;

namespace LENA.Application.Features.Grocery.GroceryLists.Validators
{
    public class AddGroceryListItemCommandValidator : AbstractValidator<AddGroceryListItemCommand>
    {
        public AddGroceryListItemCommandValidator()
        {
            RuleFor(x => x.GroceryListItem).NotNull().WithMessage("Grocery list item is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.GroceryListItem.GroceryListID).GreaterThan(0).WithMessage("Grocery list ID is required");
                    RuleFor(x => x.GroceryListItem.QuantityNeeded).GreaterThan(0).WithMessage("Quantity needed must be greater than 0");
                    RuleFor(x => x.GroceryListItem.Source).NotEmpty().WithMessage("Source is required");
                });
        }
    }
}
