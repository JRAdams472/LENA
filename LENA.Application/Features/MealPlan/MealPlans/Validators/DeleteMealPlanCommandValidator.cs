using FluentValidation;
using LENA.Application.Features.MealPlan.MealPlans.Commands;

namespace LENA.Application.Features.MealPlan.MealPlans.Validators
{
    public class DeleteMealPlanCommandValidator : AbstractValidator<DeleteMealPlanCommand>
    {
        public DeleteMealPlanCommandValidator()
        {
            RuleFor(x => x.MealPlanId).GreaterThan(0).WithMessage("Meal plan ID is required");
        }
    }
}
