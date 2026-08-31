using FluentAssertions;
using Moq;
using LENA.Application.Contracts.Persistence;
using LENA.Domain.Entity.Wine;
using TypeEntity = LENA.Domain.Entity.Wine.Type;
using LENA.Application.Features.Wine.Types.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Types
{
    public class TypesQueriesHandlerTests
    {
    [Fact]
    public async Task GetTypeByIdQuery_Should_Call_GetByIdAsync()
    {
        // Arrange
        var request = new GetTypeByIdQuery(1);
        var mockRepo = new Mock<ITypeRepository>();
        
        mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.TypeId))).ReturnsAsync(new TypeEntity { TypeName = "Test" });
        var handler = new GetTypeByIdQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.TypeId)), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTypeByNameQuery_Should_Call_GetByNameAsync()
    {
        // Arrange
        var request = new GetTypeByNameQuery("test");
        var mockRepo = new Mock<ITypeRepository>();
        
        mockRepo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name))).ReturnsAsync(new TypeEntity { TypeName = "Test" });
        var handler = new GetTypeByNameQueryHandler(mockRepo.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        mockRepo.Verify(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name)), Times.Once);
        result.Should().NotBeNull();
    }
    }
}