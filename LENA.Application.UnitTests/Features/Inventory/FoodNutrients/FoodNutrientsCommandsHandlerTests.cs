using FluentAssertions;
using Moq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using LENA.Application.Features.Inventory.FoodNutrients.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FoodNutrients
{
    public class FoodNutrientsCommandsHandlerTests
    {
    [Fact]
    public async Task CreateFoodNutrientCommand_Should_Call_CreateAsync()
    {
        // Arrange
        var request = new CreateFoodNutrientCommand(new FoodNutrient());
        var mockRepo = new Mock<IFoodNutrientRepository>();
        
        mockRepo.Setup(r => r.CreateAsync(It.Is<FoodNutrient>(x => x == request.FoodNutrient))).ReturnsAsync(new FoodNutrient());
        var handler = new CreateFoodNutrientCommandHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.CreateAsync(It.Is<FoodNutrient>(x => x == request.FoodNutrient)), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteFoodNutrientCommand_Should_Call_DeleteAsync()
    {
        // Arrange
        var request = new DeleteFoodNutrientCommand(1, 2);
        var mockRepo = new Mock<IFoodNutrientRepository>();
        mockRepo.Setup(r => r.GetByFoodAndNutrientIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new FoodNutrient());
        mockRepo.Setup(r => r.DeleteAsync(It.IsAny<FoodNutrient>())).ReturnsAsync(new FoodNutrient());
        var handler = new DeleteFoodNutrientCommandHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.DeleteAsync(It.IsAny<FoodNutrient>()), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateFoodNutrientCommand_Should_Call_UpdateAsync()
    {
        // Arrange
        var request = new UpdateFoodNutrientCommand(new FoodNutrient());
        var mockRepo = new Mock<IFoodNutrientRepository>();
        
        mockRepo.Setup(r => r.UpdateAsync(It.Is<FoodNutrient>(x => x == request.FoodNutrient))).ReturnsAsync(new FoodNutrient());
        var handler = new UpdateFoodNutrientCommandHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.UpdateAsync(It.Is<FoodNutrient>(x => x == request.FoodNutrient)), Times.Once);
        result.Should().NotBeNull();
    }
    }
}