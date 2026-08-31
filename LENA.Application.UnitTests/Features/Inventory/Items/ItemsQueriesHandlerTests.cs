using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Inventory.Items.Queries;
using LENA.Domain.Entity.Inventory;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.Items
{
    public class ItemsQueriesHandlerTests
    {
        [Fact]
        public async Task GetItemByIdQuery_Should_Call_GetByIdAsync()
        {
            // Arrange
            var request = new GetItemByIdQuery(1);
            var mockRepo = new Mock<IItemRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(x => x == request.ItemId))).ReturnsAsync(new Item { Name = "Test", Unit = "ea" });
            var handler = new GetItemByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.Is<int>(x => x == request.ItemId)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetItemByNameQuery_Should_Call_GetByNameAsync()
        {
            // Arrange
            var request = new GetItemByNameQuery("test");
            var mockRepo = new Mock<IItemRepository>();

            mockRepo.Setup(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name))).ReturnsAsync(new Item { Name = "Test", Unit = "ea" });
            var handler = new GetItemByNameQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.GetByNameAsync(It.Is<string>(x => x == request.Name)), Times.Once);
            result.Should().NotBeNull();
        }
    }
}
