using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
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

Assert.NotNull(            result);
Assert.Equal(1,             result!.GroceryListID);
        }

        [Fact]
        public async Task Handle_Should_Return_Items_With_Joined_ItemName()
        {
            var list = new GroceryList
            {
                GroceryListID = 1,
                GeneratedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                GroceryListItems = new List<GroceryListItem>
                {
                    new() { GroceryListItemID = 1, ItemID = 7, ItemName = "Flour", Source = "MealPlan" },
                    new() { GroceryListItemID = 2, ItemID = null, ManualItemName = "Napkins", Source = "Manual" }
                }
            };

            _repository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            var handler = new GetGroceryListByIdQueryHandler(_repository.Object);
            var result = await handler.Handle(new GetGroceryListByIdQuery(1), CancellationToken.None);

            Assert.Collection(
                result!.GroceryListItems!,
                first =>
                {
                    Assert.Equal("Flour", first.ItemName);
                    Assert.Null(first.ManualItemName);
                },
                second =>
                {
                    Assert.Null(second.ItemName);
                    Assert.Equal("Napkins", second.ManualItemName);
                });
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Not_Found()
        {
            _repository
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((GroceryList?)null);

            var handler = new GetGroceryListByIdQueryHandler(_repository.Object);
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetGroceryListByIdQuery(1), CancellationToken.None));
        }
    }
}
