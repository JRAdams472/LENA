using FluentValidation;

using LENA.Application.Features.MealPlan.MealPlans.Commands;

namespace LENA.Application.Features.MealPlan.MealPlans.Validators
{
    public class CreateMealPlanCommandValidator : AbstractValidator<CreateMealPlanCommand>
    {
        public CreateMealPlanCommandValidator()
        {
            RuleFor(x => x.MealPlan).NotNull().WithMessage("Meal plan is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.MealPlan.PlanName).NotEmpty().WithMessage("Plan name is required");
                    RuleFor(x => x.MealPlan.WeekStartDate).NotEmpty().WithMessage("Week start date is required");
                });
        }
    }
}