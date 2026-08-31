using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Vintages.Commands;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Vintages
{
    public class VintagesCommandsHandlerTests
    {
        [Fact]
        public async Task CreateVintageCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateVintageCommand(new Vintage());
            var mockRepo = new Mock<IVintageRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<Vintage>(x => x == request.Vintage))).ReturnsAsync(new Vintage());
            var handler = new CreateVintageCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<Vintage>(x => x == request.Vintage)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteVintageCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteVintageCommand(1);
            var mockRepo = new Mock<IVintageRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Vintage());
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Vintage>())).ReturnsAsync(new Vintage());
            var handler = new DeleteVintageCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Vintage>()), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateVintageCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateVintageCommand(new Vintage());
            var mockRepo = new Mock<IVintageRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<Vintage>(x => x == request.Vintage))).ReturnsAsync(new Vintage());
            var handler = new UpdateVintageCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Vintage>(x => x == request.Vintage)), Times.Once);
            result.Should().NotBeNull();
        }
    }
}
