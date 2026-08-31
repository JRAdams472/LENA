namespace LENA.Application.Features.MealPlan.Queries
{
    public record DailyNutritionDto(int DayOfWeek, IReadOnlyList<NutrientAmount> Nutrients);
}
