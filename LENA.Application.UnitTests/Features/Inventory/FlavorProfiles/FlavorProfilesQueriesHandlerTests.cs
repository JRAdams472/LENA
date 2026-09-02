using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Inventory.FlavorProfiles.Queries;
using LENA.Domain.Entity.Inventory;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FlavorProfiles
{
    public class FlavorProfilesQueriesHandlerTests
    {
        [Fact]
        public async Task GetFlavorProfileByIdQuery_Should_Call_GetByIdAsync()
        {
            // Arrange
            var request = new GetFlavorProfileByIdQuery(1);
            var mockRepo = new Mock<IFlavorProfileRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.FlavorId))).ReturnsAsync(new FlavorProfile { FlavorName = "Test" });
            var handler = new GetFlavorProfileByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.FlavorId)), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task GetFlavorProfileByNameQuery_Should_Call_GetByNameAsync()
        {
            // Arrange
            var request = new GetFlavorProfileByNameQuery("test");
            var mockRepo = new Mock<IFlavorProfileRepository>();

            mockRepo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name))).ReturnsAsync(new FlavorProfile { FlavorName = "Test" });
            var handler = new GetFlavorProfileByNameQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name)), Times.Once);
Assert.NotNull(            result);
        }
    }
}
