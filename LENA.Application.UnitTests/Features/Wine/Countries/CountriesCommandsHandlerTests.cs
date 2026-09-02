using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Countries
{
    public class CountriesCommandsHandlerTests
    {
        [Fact]
        public async Task CreateCountryCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateCountryCommand(new Country { CountryName = "Test", ISOCode = "XX" });
            var mockRepo = new Mock<ICountryRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<Country>(x => x == request.Country))).ReturnsAsync(new Country { CountryName = "Test", ISOCode = "XX" });
            var handler = new CreateCountryCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<Country>(x => x == request.Country)), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task DeleteCountryCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteCountryCommand(1);
            var mockRepo = new Mock<ICountryRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Country { CountryName = "Test", ISOCode = "XX" });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Country>())).ReturnsAsync(new Country { CountryName = "Test", ISOCode = "XX" });
            var handler = new DeleteCountryCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Country>()), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task UpdateCountryCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateCountryCommand(new Country { CountryName = "Test", ISOCode = "XX" });
            var mockRepo = new Mock<ICountryRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<Country>(x => x == request.Country))).ReturnsAsync(new Country { CountryName = "Test", ISOCode = "XX" });
            var handler = new UpdateCountryCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Country>(x => x == request.Country)), Times.Once);
Assert.NotNull(            result);
        }
    }
}
