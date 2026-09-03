namespace LENA.Application.Contracts.Persistence
{
    public class MealPlanNutritionRow
    {
        public byte DayOfWeek { get; set; }
        public byte? MealType { get; set; }
        public int? MealSlotId { get; set; }
        public int NutrientId { get; set; }
        public string NutrientName { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsDailyTotal { get; set; }
    }
}