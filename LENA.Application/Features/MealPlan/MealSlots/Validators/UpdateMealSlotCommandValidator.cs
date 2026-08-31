using FluentValidation;
using LENA.Application.Features.MealPlan.MealSlots.Commands;

namespace LENA.Application.Features.MealPlan.MealSlots.Validators
{
    public class UpdateMealSlotCommandValidator : AbstractValidator<UpdateMealSlotCommand>
    {
        public UpdateMealSlotCommandValidator()
        {
            RuleFor(x => x.MealSlot).NotNull().WithMessage("Meal slot is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.MealSlot.MealSlotID).GreaterThan(0).WithMessage("Meal slot ID is required");
                    RuleFor(x => x.MealSlot.MealPlanID).GreaterThan(0).WithMessage("Meal plan ID is required");
                    RuleFor(x => x.MealSlot.DayOfWeek).LessThanOrEqualTo((byte)6).WithMessage("Day of week must be 0-6");
                    RuleFor(x => x.MealSlot.MealType).LessThanOrEqualTo((byte)2).WithMessage("Meal type must be 0-2");
                });
        }
    }
}
