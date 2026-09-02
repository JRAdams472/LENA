using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Regions.Queries;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Regions
{
    public class RegionsQueriesHandlerTests
    {
        [Fact]
        public async Task GetRegionByIdQuery_Should_Call_GetByIdAsync()
        {
            // Arrange
            var request = new GetRegionByIdQuery(1);
            var mockRepo = new Mock<IRegionRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.RegionId))).ReturnsAsync(new Region { RegionName = "Test" });
            var handler = new GetRegionByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.RegionId)), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task GetRegionByNameAndCountryIdQuery_Should_Call_GetByNameAndCountryIdAsync()
        {
            // Arrange
            var request = new GetRegionByNameAndCountryIdQuery("test", 1);
            var mockRepo = new Mock<IRegionRepository>();

            mockRepo.Setup(r => r.GetByNameAndCountryIdAsync(It.Is<string>(x => x == request.Name), It.Is<int>(x => x == request.CountryId))).ReturnsAsync(new Region { RegionName = "Test" });
            var handler = new GetRegionByNameAndCountryIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByNameAndCountryIdAsync(It.Is<string>(x => x == request.Name), It.Is<int>(x => x == request.CountryId)), Times.Once);
Assert.NotNull(            result);
        }
    }
}
