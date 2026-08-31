using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Bottles.Queries;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Bottles
{
    public class BottlesQueriesHandlerTests
    {
        [Fact]
        public async Task GetBottleByIdQuery_Should_Call_GetByIdAsync()
        {
            // Arrange
            var request = new GetBottleByIdQuery(1);
            var mockRepo = new Mock<IBottleRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.BottleId))).ReturnsAsync(new Bottle());
            var handler = new GetBottleByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.BottleId)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetTotalBottleCountQuery_Should_Call_GetTotalBottleCountAsync()
        {
            // Arrange
            var request = new GetTotalBottleCountQuery();
            var mockRepo = new Mock<IBottleRepository>();

            mockRepo.Setup(r => r.GetTotalBottleCountAsync()).ReturnsAsync(1);
            var handler = new GetTotalBottleCountQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetTotalBottleCountAsync(), Times.Once);
            result.Should().Be(1);
        }
    }
}
