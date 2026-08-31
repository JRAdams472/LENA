using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Regions
{
    public class RegionsCommandsHandlerTests
    {
        [Fact]
        public async Task CreateRegionCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateRegionCommand(new Region { RegionName = "Test" });
            var mockRepo = new Mock<IRegionRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<Region>(x => x == request.Region))).ReturnsAsync(new Region { RegionName = "Test" });
            var handler = new CreateRegionCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<Region>(x => x == request.Region)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteRegionCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteRegionCommand(1);
            var mockRepo = new Mock<IRegionRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Region { RegionName = "Test" });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Region>())).ReturnsAsync(new Region { RegionName = "Test" });
            var handler = new DeleteRegionCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Region>()), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateRegionCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateRegionCommand(new Region { RegionName = "Test" });
            var mockRepo = new Mock<IRegionRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<Region>(x => x == request.Region))).ReturnsAsync(new Region { RegionName = "Test" });
            var handler = new UpdateRegionCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Region>(x => x == request.Region)), Times.Once);
            result.Should().NotBeNull();
        }
    }
}
