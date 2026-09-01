using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Grocery.GroceryLists.Queries;
using LENA.Domain.Entity.Grocery;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Grocery.GroceryLists.Queries
{
    public class GetGroceryListByIdQueryTests
    {
        private readonly Mock<IGroceryListRepository> _repository = new();

        [Fact]
        public async Task Handle_Should_Return_List_When_Found()
        {
            var list = new GroceryList
            {
                GroceryListID = 1,
                GeneratedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            _repository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            var handler = new GetGroceryListByIdQueryHandler(_repository.Object);
            var result = await handler.Handle(new GetGroceryListByIdQuery(1), CancellationToken.None);

            result.Should().NotBeNull();
            result!.GroceryListID.Should().Be(1);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Not_Found()
        {
            _repository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryList?)null);

            var handler = new GetGroceryListByIdQueryHandler(_repository.Object);
            var result = await handler.Handle(new GetGroceryListByIdQuery(1), CancellationToken.None);

            result.Should().BeNull();
        }
    }
}
