using FluentValidation;

using LENA.Application.Features.MealPlan.MealSlotItems.Commands;

namespace LENA.Application.Features.MealPlan.MealSlotItems.Validators
{
    public class CreateMealSlotItemCommandValidator : AbstractValidator<CreateMealSlotItemCommand>
    {
        public CreateMealSlotItemCommandValidator()
        {
            RuleFor(x => x.MealSlotItem).NotNull().WithMessage("Meal slot item is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.MealSlotItem.MealSlotID).GreaterThan(0).WithMessage("Meal slot ID is required");
                    RuleFor(x => x.MealSlotItem.ItemID).GreaterThan(0).WithMessage("Item ID is required");
                    RuleFor(x => x.MealSlotItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
                });
        }
    }
}