using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Contracts.Inventory;
using LENA.API.Controllers;
using LENA.Application.Exceptions;
using LENA.Application.Features.Inventory.Items.Commands;
using LENA.Application.Features.Inventory.Items.Queries;
using LENA.Application.Models;
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
            _mediator.Setup(m => m.Send(It.IsAny<GetItemsPagedQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<Item> { Items = new List<Item>() });

#pragma warning disable CS0618
            var result = await _sut.GetItems();
#pragma warning restore CS0618

            _mediator.Verify(m => m.Send(It.Is<GetItemsPagedQuery>(q => q.PageNumber == 1 && q.PageSize == 25), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetItemById_Should_Return_Ok_When_Found()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetItemByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Item { Name = "x", Unit = "y" });

            var result = await _sut.GetItemById(1);

            _mediator.Verify(m => m.Send(It.Is<GetItemByIdQuery>(q => q.ItemId == 1), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetItemById_Should_Throw_NotFound_When_Missing()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetItemByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException(nameof(Item), 1));

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetItemById(1));
        }

        [Fact]
        public async Task CreateItem_Should_Return_CreatedAtAction_And_Send_CreateItemCommand()
        {
            var item = new Item { Name = "x", Unit = "y" };
            _mediator.Setup(m => m.Send(It.IsAny<CreateItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(item);

            var result = await _sut.CreateItem(new CreateItemRequest { Name = item.Name, Unit = item.Unit });

            _mediator.Verify(m => m.Send(It.Is<CreateItemCommand>(c => c.Item != null), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
    }
}