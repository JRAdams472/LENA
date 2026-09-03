using FluentValidation;

using LENA.Application.Features.MealPlan.MealSlots.Commands;

namespace LENA.Application.Features.MealPlan.MealSlots.Validators
{
    public class DeleteMealSlotCommandValidator : AbstractValidator<DeleteMealSlotCommand>
    {
        public DeleteMealSlotCommandValidator()
        {
            RuleFor(x => x.MealSlotId).GreaterThan(0).WithMessage("Meal slot ID is required");
        }
    }
}