using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Domain.Entity.Wine;

using Moq;

using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Bottles
{
    public class BottlesCommandsHandlerTests
    {
        [Fact]
        public async Task CreateBottleCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateBottleCommand(new Bottle());
            var mockRepo = new Mock<IBottleRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<Bottle>(x => x == request.Bottle))).ReturnsAsync(new Bottle());
            var handler = new CreateBottleCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<Bottle>(x => x == request.Bottle)), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteBottleCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteBottleCommand(1);
            var mockRepo = new Mock<IBottleRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Bottle());
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Bottle>())).ReturnsAsync(new Bottle());
            var handler = new DeleteBottleCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Bottle>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateBottleCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateBottleCommand(new Bottle());
            var mockRepo = new Mock<IBottleRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<Bottle>(x => x == request.Bottle))).ReturnsAsync(new Bottle());
            var handler = new UpdateBottleCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Bottle>(x => x == request.Bottle)), Times.Once);
            Assert.NotNull(result);
        }
    }
}