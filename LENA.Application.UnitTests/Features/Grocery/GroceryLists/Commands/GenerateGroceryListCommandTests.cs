using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Grocery.GroceryLists.Commands;
using LENA.Domain.Entity.Grocery;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Grocery.GroceryLists.Commands
{
    public class GenerateGroceryListCommandTests
    {
        private readonly Mock<IGroceryListRepository> _repository = new();

        [Fact]
        public async Task Handle_Should_Call_Repository_With_MealPlanId()
        {
            _repository
                .Setup(r => r.GenerateFromMealPlanAsync(It.IsAny<GroceryList>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryList list, CancellationToken _) =>
                {
                    list.GroceryListID = 1;
                    return list;
                });

            var handler = new GenerateGroceryListCommandHandler(_repository.Object);
            var result = await handler.Handle(new GenerateGroceryListCommand(5), CancellationToken.None);

            _repository.Verify(
                r => r.GenerateFromMealPlanAsync(
                    It.Is<GroceryList>(g => g.MealPlanID == 5),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            result.GroceryListID.Should().Be(1);
        }

        [Fact]
        public async Task Handle_Should_Allow_Null_MealPlanId()
        {
            _repository
                .Setup(r => r.GenerateFromMealPlanAsync(It.IsAny<GroceryList>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryList list, CancellationToken _) =>
                {
                    list.GroceryListID = 2;
                    return list;
                });

            var handler = new GenerateGroceryListCommandHandler(_repository.Object);
            var result = await handler.Handle(new GenerateGroceryListCommand(null), CancellationToken.None);

            _repository.Verify(
                r => r.GenerateFromMealPlanAsync(
                    It.Is<GroceryList>(g => g.MealPlanID == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            result.GroceryListID.Should().Be(2);
        }
    }
}
