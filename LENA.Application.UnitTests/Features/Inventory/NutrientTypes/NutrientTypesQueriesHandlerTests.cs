using FluentAssertions;
using Moq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Inventory;
using LENA.Application.Features.Inventory.NutrientTypes.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.NutrientTypes
{
    public class NutrientTypesQueriesHandlerTests
    {
    [Fact]
    public async Task GetNutrientTypeByIdQuery_Should_Call_GetByIdAsync()
    {
        // Arrange
        var request = new GetNutrientTypeByIdQuery(1);
        var mockRepo = new Mock<INutrientTypeRepository>();
        
        mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.NutrientTypeId))).ReturnsAsync(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
        var handler = new GetNutrientTypeByIdQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.NutrientTypeId)), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNutrientTypeByNameQuery_Should_Call_GetByNameAsync()
    {
        // Arrange
        var request = new GetNutrientTypeByNameQuery("test");
        var mockRepo = new Mock<INutrientTypeRepository>();
        
        mockRepo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name))).ReturnsAsync(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
        var handler = new GetNutrientTypeByNameQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name)), Times.Once);
        result.Should().NotBeNull();
    }
    }
}