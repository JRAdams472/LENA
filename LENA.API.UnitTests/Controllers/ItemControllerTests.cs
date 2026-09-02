using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Controllers;
using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Application.Features.Inventory.Items.Queries;
using LENA.Domain.Entity.Inventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class ItemControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly ItemController _sut;

        public ItemControllerTests() => _sut = new ItemController(_mediator.Object);

        [Fact]
        public async Task GetItems_Should_Return_Ok_And_Send_GetItemsQuery()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetItemsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Item>());

            var result = await _sut.GetItems();

            _mediator.Verify(m => m.Send(It.IsAny<GetItemsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task GetItemById_Should_Return_Ok_When_Found()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetItemByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Item { Name = "x", Unit = "y" });

            var result = await _sut.GetItemById(1);

            _mediator.Verify(m => m.Send(It.Is<GetItemByIdQuery>(q => q.ItemId == 1), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task CreateItem_Should_Return_CreatedAtAction_And_Send_CreateItemCommand()
        {
            var item = new Item { Name = "x", Unit = "y" };
            _mediator.Setup(m => m.Send(It.IsAny<CreateItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(item);

            var result = await _sut.CreateItem(item);

            _mediator.Verify(m => m.Send(It.Is<CreateItemCommand>(c => c.Item == item), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<CreatedAtActionResult>(            result.Result);
        }
    }
}
