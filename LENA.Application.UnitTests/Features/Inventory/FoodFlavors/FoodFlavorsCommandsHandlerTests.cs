using FluentAssertions;
using Moq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using LENA.Application.Features.Inventory.FoodFlavors.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodFlavors
{
    public class FoodFlavorsCommandsHandlerTests
    {
    [Fact]
    public async Task CreateFoodFlavorCommand_Should_Call_CreateAsync()
    {
        // Arrange
        var request = new CreateFoodFlavorCommand(new FoodFlavor());
        var mockRepo = new Mock<IFoodFlavorRepository>();
        
        mockRepo.Setup(r => r.CreateAsync(It.Is<FoodFlavor>(x => x == request.FoodFlavor))).ReturnsAsync(new FoodFlavor());
        var handler = new CreateFoodFlavorCommandHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.CreateAsync(It.Is<FoodFlavor>(x => x == request.FoodFlavor)), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteFoodFlavorCommand_Should_Call_DeleteAsync()
    {
        // Arrange
        var request = new DeleteFoodFlavorCommand(1);
        var mockRepo = new Mock<IFoodFlavorRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new FoodFlavor());
        mockRepo.Setup(r => r.DeleteAsync(It.IsAny<FoodFlavor>())).ReturnsAsync(new FoodFlavor());
        var handler = new DeleteFoodFlavorCommandHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.DeleteAsync(It.IsAny<FoodFlavor>()), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateFoodFlavorCommand_Should_Call_UpdateAsync()
    {
        // Arrange
        var request = new UpdateFoodFlavorCommand(new FoodFlavor());
        var mockRepo = new Mock<IFoodFlavorRepository>();
        
        mockRepo.Setup(r => r.UpdateAsync(It.Is<FoodFlavor>(x => x == request.FoodFlavor))).ReturnsAsync(new FoodFlavor());
        var handler = new UpdateFoodFlavorCommandHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.UpdateAsync(It.Is<FoodFlavor>(x => x == request.FoodFlavor)), Times.Once);
        result.Should().NotBeNull();
    }
    }
}