using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Wine.Types.Commands;
using LENA.Domain.Entity.Wine;
using Moq;
using Xunit;
using TypeEntity = LENA.Domain.Entity.Wine.Type;

namespace LENA.Application.UnitTests.Features.Wine.Types
{
    public class TypesCommandsHandlerTests
    {
        [Fact]
        public async Task CreateTypeCommand_Should_Call_CreateAsync()
        {
            // Arrange
            var request = new CreateTypeCommand(new TypeEntity { TypeName = "Test" });
            var mockRepo = new Mock<ITypeRepository>();

            mockRepo.Setup(r => r.CreateAsync(It.Is<TypeEntity>(x => x == request.Type))).ReturnsAsync(new TypeEntity { TypeName = "Test" });
            var handler = new CreateTypeCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.CreateAsync(It.Is<TypeEntity>(x => x == request.Type)), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task DeleteTypeCommand_Should_Call_DeleteAsync()
        {
            // Arrange
            var request = new DeleteTypeCommand(1);
            var mockRepo = new Mock<ITypeRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TypeEntity { TypeName = "Test" });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<TypeEntity>())).ReturnsAsync(new TypeEntity { TypeName = "Test" });
            var handler = new DeleteTypeCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<TypeEntity>()), Times.Once);
Assert.NotNull(            result);
        }

        [Fact]
        public async Task UpdateTypeCommand_Should_Call_UpdateAsync()
        {
            // Arrange
            var request = new UpdateTypeCommand(new TypeEntity { TypeName = "Test" });
            var mockRepo = new Mock<ITypeRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.Is<TypeEntity>(x => x == request.Type))).ReturnsAsync(new TypeEntity { TypeName = "Test" });
            var handler = new UpdateTypeCommandHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<TypeEntity>(x => x == request.Type)), Times.Once);
Assert.NotNull(            result);
        }
    }
}
