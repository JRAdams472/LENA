using FluentAssertions;
using LENA.API.Controllers;
using LENA.Application.Features.Wine.Bottles.Commands;
using LENA.Application.Features.Wine.Bottles.Queries;
using LENA.Domain.Entity.Wine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LENA.API.UnitTests.Controllers
{
    public class WineControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly WineController _sut;

        public WineControllerTests() => _sut = new WineController(_mediator.Object);

        [Fact]
        public async Task GetBottles_Should_Return_Ok_And_Send_GetBottlesQuery()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetBottlesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Bottle>().AsReadOnly());

            var result = await _sut.GetBottles();

            _mediator.Verify(m => m.Send(It.IsAny<GetBottlesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetBottleById_Should_Return_Ok_When_Found()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetBottleByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Bottle());

            var result = await _sut.GetBottleById(1);

            _mediator.Verify(m => m.Send(It.Is<GetBottleByIdQuery>(q => q.BottleId == 1), It.IsAny<CancellationToken>()), Times.Once);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateBottle_Should_Return_CreatedAtAction_And_Send_CreateBottleCommand()
        {
            var bottle = new Bottle();
            _mediator.Setup(m => m.Send(It.IsAny<CreateBottleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bottle);

            var result = await _sut.CreateBottle(bottle);

            _mediator.Verify(m => m.Send(It.Is<CreateBottleCommand>(c => c.Bottle == bottle), It.IsAny<CancellationToken>()), Times.Once);
            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }
    }
}
