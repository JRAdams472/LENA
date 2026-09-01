using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Domain.Entity.Inventory;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.Items
{
    public class ItemsCommandsHandlerTests
    {
        [Fact]
        public async Task CreateItemCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateItemCommand(new Item { Name = "Test", Unit = "ea" });
            var mockRepo = new Mock<IItemRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<Item>(x => x == request.Item))).ReturnsAsync(new Item { Name = "Test", Unit = "ea" });
            var handler = new CreateItemCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<Item>(x => x == request.Item)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteItemCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteItemCommand(1);
            var mockRepo = new Mock<IItemRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Item { Name = "Test", Unit = "ea" });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Item>())).ReturnsAsync(new Item { Name = "Test", Unit = "ea" });
            var handler = new DeleteItemCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Item>()), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateItemCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateItemCommand(new Item { Name = "Test", Unit = "ea" });
            var mockRepo = new Mock<IItemRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<Item>(x => x == request.Item))).ReturnsAsync(new Item { Name = "Test", Unit = "ea" });
            var handler = new UpdateItemCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Item>(x => x == request.Item)), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AdjustItemQuantityCommand_Should_Forward_Audit_User()
        {
            var request = new AdjustItemQuantityCommand(4, 0m, null);
            request.AuditableEntity.LastUpdatedBy = "tester";

            var mockRepo = new Mock<IItemRepository>();
            var handler = new AdjustItemQuantityCommandHandler(mockRepo.Object);

            await handler.Handle(request, CancellationToken.None);

            mockRepo.Verify(
                r => r.AdjustQuantityAsync(4, 0m, null, "tester", It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
