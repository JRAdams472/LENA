using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Contracts.Grocery;
using LENA.API.Controllers;
using LENA.Application.Exceptions;
using LENA.Application.Features.Grocery.GroceryLists.Commands;
using LENA.Application.Features.Grocery.GroceryLists.Queries;
using LENA.Application.Models;
using LENA.Domain.Entity.Grocery;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class GroceryListControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly GroceryListController _sut;

        public GroceryListControllerTests() => _sut = new GroceryListController(_mediator.Object);

        [Fact]
        public async Task GetGroceryLists_Should_Return_Ok()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetGroceryListsPagedQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<GroceryList> { Items = new List<GroceryList>() });

            var result = await _sut.GetGroceryLists();

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetGroceryListById_Should_Throw_NotFound_When_Missing()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetGroceryListByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException(nameof(GroceryList), 1));

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetGroceryListById(1));
        }

        [Fact]
        public async Task GenerateGroceryList_Should_Return_CreatedAtAction()
        {
            var list = new GroceryList { GroceryListID = 1 };
            _mediator.Setup(m => m.Send(It.IsAny<GenerateGroceryListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            var result = await _sut.GenerateGroceryList(5);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task AddGroceryListItem_Should_Map_To_Command_And_Set_Source_To_Manual_When_Blank()
        {
            GroceryListItem? sent = null;
            _mediator.Setup(m => m.Send(It.IsAny<AddGroceryListItemCommand>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((c, _) => sent = ((AddGroceryListItemCommand)c).GroceryListItem)
                .ReturnsAsync(new GroceryListItem { GroceryListItemID = 1 });

            await _sut.AddGroceryListItem(1, new CreateGroceryListItemRequest { ManualItemName = "Eggs", QuantityNeeded = 12 });

            Assert.NotNull(sent);
            Assert.Equal(1, sent!.GroceryListID);
            Assert.Equal("Manual", sent.Source);
        }

        [Fact]
        public async Task ToggleGroceryItemChecked_Should_Send_Command()
        {
            _mediator.Setup(m => m.Send(It.IsAny<ToggleGroceryListItemCheckedCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GroceryListItem { GroceryListItemID = 1 });

            var result = await _sut.ToggleGroceryItemChecked(1);

            _mediator.Verify(m => m.Send(It.Is<ToggleGroceryListItemCheckedCommand>(c => c.GroceryListItemId == 1), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteGroceryItem_Should_Return_NoContent()
        {
            var result = await _sut.DeleteGroceryItem(1);

            _mediator.Verify(m => m.Send(It.Is<DeleteGroceryListItemCommand>(c => c.GroceryListItemId == 1), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}