namespace LENA.Application.Features.MealPlan.Queries
{
    public record MealNutritionDto(int DayOfWeek, int MealType, int MealSlotId, IReadOnlyList<NutrientAmount> Nutrients);
}