using FluentAssertions;
using Moq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using LENA.Application.Features.Inventory.FoodNutrients.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodNutrients
{
    public class FoodNutrientsQueriesHandlerTests
    {
    [Fact]
    public async Task GetFoodNutrientByFoodAndNutrientIdQuery_Should_Call_GetByFoodAndNutrientIdAsync()
    {
        // Arrange
        var request = new GetFoodNutrientByFoodAndNutrientIdQuery(1, 1);
        var mockRepo = new Mock<IFoodNutrientRepository>();
        
        mockRepo.Setup(r => r.GetByFoodAndNutrientIdAsync(It.Is<int>(x => x == request.FoodId), It.Is<int>(x => x == request.NutrientId))).ReturnsAsync(new FoodNutrient());
        var handler = new GetFoodNutrientByFoodAndNutrientIdQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByFoodAndNutrientIdAsync(It.Is<int>(x => x == request.FoodId), It.Is<int>(x => x == request.NutrientId)), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFoodNutrientByIdQuery_Should_Call_GetByIdAsync()
    {
        // Arrange
        var request = new GetFoodNutrientByIdQuery(1);
        var mockRepo = new Mock<IFoodNutrientRepository>();
        
        mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.FoodNutrientId))).ReturnsAsync(new FoodNutrient());
        var handler = new GetFoodNutrientByIdQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.FoodNutrientId)), Times.Once);
        result.Should().NotBeNull();
    }
    }
}