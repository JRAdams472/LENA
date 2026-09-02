using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.MealPlan.Queries;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.MealPlan.Queries
{
    public class GetMealPlanNutritionQueryTests
    {
        private readonly Mock<IMealPlanRepository> _repository = new();

        [Fact]
        public async Task Handle_Should_Return_Aggregated_Nutrition()
        {
            var rows = new List<MealPlanNutritionRow>
            {
                new()
                {
                    DayOfWeek = 0,
                    MealSlotId = 1,
                    MealType = 0,
                    NutrientId = 1,
                    NutrientName = "Protein",
                    UnitOfMeasure = "g",
                    Amount = 10m,
                    IsDailyTotal = false
                },
                new()
                {
                    DayOfWeek = 0,
                    IsDailyTotal = true,
                    NutrientId = 1,
                    NutrientName = "Protein",
                    UnitOfMeasure = "g",
                    Amount = 10m
                }
            };

            _repository
                .Setup(r => r.GetMealPlanNutritionAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(rows);

            var handler = new GetMealPlanNutritionQueryHandler(_repository.Object);
            var result = await handler.Handle(new GetMealPlanNutritionQuery(1), CancellationToken.None);

Assert.NotNull(            result);
Assert.Equal(1,             result.MealPlanId);
Assert.Single(            result.DailyTotals);
Assert.Single(            result.Meals);
Assert.Single(            result.Meals[0].Nutrients, n => n.NutrientName == "Protein");
        }
    }
}
