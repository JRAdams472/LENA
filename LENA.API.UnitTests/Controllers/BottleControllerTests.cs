using LENA.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Controllers;
using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Application.Features.Wine.Bottles.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class BottleControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly BottleController _sut;

        public BottleControllerTests() => _sut = new BottleController(_mediator.Object);

        [Fact]
        public async Task GetBottles_Should_Return_Ok_And_Send_GetBottlesQuery()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetBottlesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Bottle>());

            var result = await _sut.GetBottles();

            _mediator.Verify(m => m.Send(It.IsAny<GetBottlesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task GetBottlesPaged_Should_Call_Mediator_With_Defaults()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetBottlesPagedQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<Bottle> { Items = new List<Bottle>() });

            var result = await _sut.GetBottlesPaged();

            _mediator.Verify(m => m.Send(It.Is<GetBottlesPagedQuery>(q => q.PageNumber == 1 && q.PageSize == 25), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task GetBottlesPaged_Should_Call_Mediator_With_Supplied_PageNumber_And_PageSize()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetBottlesPagedQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<Bottle> { Items = new List<Bottle>() });

            var result = await _sut.GetBottlesPaged(3, 50);

            _mediator.Verify(m => m.Send(It.Is<GetBottlesPagedQuery>(q => q.PageNumber == 3 && q.PageSize == 50), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task GetBottleById_Should_Return_Ok_When_Found()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetBottleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Bottle());

            var result = await _sut.GetBottleById(1);

            _mediator.Verify(m => m.Send(It.Is<GetBottleByIdQuery>(q => q.BottleId == 1), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task CreateBottle_Should_Return_CreatedAtAction_And_Send_CreateBottleCommand()
        {
            var bottle = new Bottle();
            _mediator.Setup(m => m.Send(It.IsAny<CreateBottleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bottle);

            var result = await _sut.CreateBottle(bottle);

            _mediator.Verify(m => m.Send(It.Is<CreateBottleCommand>(c => c.Bottle == bottle), It.IsAny<CancellationToken>()), Times.Once);
Assert.IsType<CreatedAtActionResult>(            result.Result);
        }
    }
}
