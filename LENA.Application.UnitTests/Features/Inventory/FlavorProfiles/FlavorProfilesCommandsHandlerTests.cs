using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Inventory.FlavorProfiles.Commands;
using LENA.Domain.Entity.Inventory;

using Moq;

using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FlavorProfiles
{
    public class FlavorProfilesCommandsHandlerTests
    {
        [Fact]
        public async Task CreateFlavorProfileCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateFlavorProfileCommand(new FlavorProfile { FlavorName = "Test" });
            var mockRepo = new Mock<IFlavorProfileRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<FlavorProfile>(x => x == request.FlavorProfile))).ReturnsAsync(new FlavorProfile { FlavorName = "Test" });
            var handler = new CreateFlavorProfileCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<FlavorProfile>(x => x == request.FlavorProfile)), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteFlavorProfileCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteFlavorProfileCommand(1);
            var mockRepo = new Mock<IFlavorProfileRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new FlavorProfile { FlavorName = "Test" });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<FlavorProfile>())).ReturnsAsync(new FlavorProfile { FlavorName = "Test" });
            var handler = new DeleteFlavorProfileCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<FlavorProfile>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateFlavorProfileCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateFlavorProfileCommand(new FlavorProfile { FlavorName = "Test" });
            var mockRepo = new Mock<IFlavorProfileRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<FlavorProfile>(x => x == request.FlavorProfile))).ReturnsAsync(new FlavorProfile { FlavorName = "Test" });
            var handler = new UpdateFlavorProfileCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<FlavorProfile>(x => x == request.FlavorProfile)), Times.Once);
            Assert.NotNull(result);
        }
    }
}