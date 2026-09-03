using FluentValidation;

using LENA.Application.Features.MealPlan.MealPlans.Commands;

namespace LENA.Application.Features.MealPlan.MealPlans.Validators
{
    public class UpdateMealPlanCommandValidator : AbstractValidator<UpdateMealPlanCommand>
    {
        public UpdateMealPlanCommandValidator()
        {
            RuleFor(x => x.MealPlan).NotNull().WithMessage("Meal plan is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.MealPlan.MealPlanID).GreaterThan(0).WithMessage("Meal plan ID is required");
                    RuleFor(x => x.MealPlan.PlanName).NotEmpty().WithMessage("Plan name is required");
                });
        }
    }
}