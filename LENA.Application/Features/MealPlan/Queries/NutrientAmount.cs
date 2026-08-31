namespace LENA.Application.Features.MealPlan.Queries
{
    public record NutrientAmount(int NutrientId, string NutrientName, string UnitOfMeasure, decimal Amount);
}
