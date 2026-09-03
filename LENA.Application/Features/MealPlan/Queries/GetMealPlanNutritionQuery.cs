using LENA.Application.Contracts.Persistence;

using MediatR;

namespace LENA.Application.Features.MealPlan.Queries
{
    public record GetMealPlanNutritionQuery(int MealPlanId) : IRequest<MealPlanNutritionDto>;

    public class GetMealPlanNutritionQueryHandler : IRequestHandler<GetMealPlanNutritionQuery, MealPlanNutritionDto>
    {
        private readonly IMealPlanRepository _mealPlanRepository;

        public GetMealPlanNutritionQueryHandler(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<MealPlanNutritionDto> Handle(GetMealPlanNutritionQuery request, CancellationToken cancellationToken)
        {
            var rows = await _mealPlanRepository.GetMealPlanNutritionAsync(request.MealPlanId, cancellationToken);

            var daily = rows
                .Where(r => r.IsDailyTotal)
                .GroupBy(r => (int)r.DayOfWeek)
                .Select(g => new DailyNutritionDto(
                    g.Key,
                    g.Select(r => new NutrientAmount(r.NutrientId, r.NutrientName, r.UnitOfMeasure, r.Amount)).ToList()))
                .ToList();

            var meals = rows
                .Where(r => !r.IsDailyTotal && r.MealType.HasValue && r.MealSlotId.HasValue)
                .GroupBy(r => new { Day = (int)r.DayOfWeek, Type = (int)r.MealType!.Value, Slot = r.MealSlotId!.Value })
                .Select(g => new MealNutritionDto(
                    g.Key.Day,
                    g.Key.Type,
                    g.Key.Slot,
                    g.Select(r => new NutrientAmount(r.NutrientId, r.NutrientName, r.UnitOfMeasure, r.Amount)).ToList()))
                .ToList();

            return new MealPlanNutritionDto(request.MealPlanId, daily, meals);
        }
    }
}