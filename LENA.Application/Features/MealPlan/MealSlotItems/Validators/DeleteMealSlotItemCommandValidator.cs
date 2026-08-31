using FluentValidation;
using LENA.Application.Features.MealPlan.MealSlotItems.Commands;

namespace LENA.Application.Features.MealPlan.MealSlotItems.Validators
{
    public class DeleteMealSlotItemCommandValidator : AbstractValidator<DeleteMealSlotItemCommand>
    {
        public DeleteMealSlotItemCommandValidator()
        {
            RuleFor(x => x.MealSlotItemId).GreaterThan(0).WithMessage("Meal slot item ID is required");
        }
    }
}
