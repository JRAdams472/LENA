using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Countries.Queries;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Countries
{
    public class CountriesQueriesHandlerTests
    {
        [Fact]
        public async Task GetCountryByIdQuery_Should_Call_GetByIdAsync()
        {
            // Arrange
            var request = new GetCountryByIdQuery(1);
            var mockRepo = new Mock<ICountryRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.CountryId))).ReturnsAsync(new Country { CountryName = "Test", ISOCode = "XX" });
            var handler = new GetCountryByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.CountryId)), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task GetCountryByISOCodeQuery_Should_Call_GetByISOCodeAsync()
        {
            // Arrange
            var request = new GetCountryByISOCodeQuery("test");
            var mockRepo = new Mock<ICountryRepository>();

            mockRepo.Setup(r => r.GetByISOCodeAsync(It.Is<string>(x => x == request.ISOCode))).ReturnsAsync(new Country { CountryName = "Test", ISOCode = "XX" });
            var handler = new GetCountryByISOCodeQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByISOCodeAsync(It.Is<string>(x => x == request.ISOCode)), Times.Once);
Assert.NotNull(            result);
        }
    }
}
