using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Inventory.NutrientTypes.Commands;
using LENA.Domain.Entity.Inventory;

using Moq;

using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.NutrientTypes
{
    public class NutrientTypesCommandsHandlerTests
    {
        [Fact]
        public async Task CreateNutrientTypeCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateNutrientTypeCommand(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var mockRepo = new Mock<INutrientTypeRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<NutrientType>(x => x == request.NutrientType))).ReturnsAsync(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var handler = new CreateNutrientTypeCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<NutrientType>(x => x == request.NutrientType)), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteNutrientTypeCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteNutrientTypeCommand(1);
            var mockRepo = new Mock<INutrientTypeRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<NutrientType>())).ReturnsAsync(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var handler = new DeleteNutrientTypeCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<NutrientType>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateNutrientTypeCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateNutrientTypeCommand(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var mockRepo = new Mock<INutrientTypeRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<NutrientType>(x => x == request.NutrientType))).ReturnsAsync(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var handler = new UpdateNutrientTypeCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<NutrientType>(x => x == request.NutrientType)), Times.Once);
            Assert.NotNull(result);
        }
    }
}