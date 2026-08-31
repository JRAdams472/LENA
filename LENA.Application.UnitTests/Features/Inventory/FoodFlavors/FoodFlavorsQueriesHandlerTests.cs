using FluentAssertions;
using Moq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using LENA.Application.Features.Inventory.FoodFlavors.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodFlavors
{
    public class FoodFlavorsQueriesHandlerTests
    {
    [Fact]
    public async Task GetFoodFlavorByFoodAndFlavorIdQuery_Should_Call_GetByFoodAndFlavorIdAsync()
    {
        // Arrange
        var request = new GetFoodFlavorByFoodAndFlavorIdQuery(1, 1);
        var mockRepo = new Mock<IFoodFlavorRepository>();
        
        mockRepo.Setup(r => r.GetByFoodAndFlavorIdAsync(It.Is<int>(x => x == request.FoodId), It.Is<int>(x => x == request.FlavorId))).ReturnsAsync(new FoodFlavor());
        var handler = new GetFoodFlavorByFoodAndFlavorIdQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByFoodAndFlavorIdAsync(It.Is<int>(x => x == request.FoodId), It.Is<int>(x => x == request.FlavorId)), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFoodFlavorByIdQuery_Should_Call_GetByIdAsync()
    {
        // Arrange
        var request = new GetFoodFlavorByIdQuery(1);
        var mockRepo = new Mock<IFoodFlavorRepository>();
        
        mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.FoodFlavorId))).ReturnsAsync(new FoodFlavor());
        var handler = new GetFoodFlavorByIdQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.FoodFlavorId)), Times.Once);
        result.Should().NotBeNull();
    }
    }
}