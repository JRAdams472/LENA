using FluentValidation;

using LENA.Application.Features.Grocery.GroceryLists.Commands;

namespace LENA.Application.Features.Grocery.GroceryLists.Validators
{
    public class DeleteGroceryListItemCommandValidator : AbstractValidator<DeleteGroceryListItemCommand>
    {
        public DeleteGroceryListItemCommandValidator()
        {
            RuleFor(x => x.GroceryListItemId).GreaterThan(0).WithMessage("Grocery list item ID is required");
        }
    }
}