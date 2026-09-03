namespace LENA.Application.Features.MealPlan.Queries
{
    public record MealPlanNutritionDto(int MealPlanId, IReadOnlyList<DailyNutritionDto> DailyTotals, IReadOnlyList<MealNutritionDto> Meals);
}