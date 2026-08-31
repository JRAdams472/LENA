using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Vintages.Queries;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Vintages
{
    public class VintagesQueriesHandlerTests
    {
        [Fact]
        public async Task GetVintageByIdQuery_Should_Call_GetByIdAsync()
        {
            // Arrange
            var request = new GetVintageByIdQuery(1);
            var mockRepo = new Mock<IVintageRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.VintageId))).ReturnsAsync(new Vintage());
            var handler = new GetVintageByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.VintageId)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetVintageByYearQuery_Should_Call_GetByYearAsync()
        {
            // Arrange
            var request = new GetVintageByYearQuery(1);
            var mockRepo = new Mock<IVintageRepository>();

            mockRepo.Setup(r => r.GetByYearAsync(It.Is<int>(x => x == request.Year))).ReturnsAsync(new Vintage());
            var handler = new GetVintageByYearQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByYearAsync(It.Is<int>(x => x == request.Year)), Times.Once);
            result.Should().NotBeNull();
        }
    }
}
