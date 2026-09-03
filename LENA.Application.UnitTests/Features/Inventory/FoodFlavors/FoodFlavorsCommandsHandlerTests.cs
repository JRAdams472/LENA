using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Inventory.FoodFlavors.Commands;
using LENA.Domain.Entity.Inventory;

using Moq;

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
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteFoodFlavorCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteFoodFlavorCommand(1, 2);
            var mockRepo = new Mock<IFoodFlavorRepository>();
            mockRepo.Setup(r => r.GetByFoodAndFlavorIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new FoodFlavor());
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<FoodFlavor>())).ReturnsAsync(new FoodFlavor());
            var handler = new DeleteFoodFlavorCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<FoodFlavor>()), Times.Once);
            Assert.NotNull(result);
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
            Assert.NotNull(result);
        }
    }
}