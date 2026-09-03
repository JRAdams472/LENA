using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Controllers;
using LENA.Application.Features.MealPlan.MealPlans.Commands;
using LENA.Application.Features.MealPlan.MealPlans.Queries;
using LENA.Application.Features.MealPlan.Queries;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.MealPlan;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MealPlanEntity = LENA.Domain.Entity.MealPlan.MealPlan;

namespace LENA.API.UnitTests.Controllers
{
    public class MealPlanControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly MealPlanController _sut;

        public MealPlanControllerTests() => _sut = new MealPlanController(_mediator.Object);

        [Fact]
        public async Task GetMealPlans_Should_Return_Ok()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetMealPlansQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MealPlanEntity>());

            var result = await _sut.GetMealPlans();

Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task GetMealPlanById_Should_Throw_NotFound_When_Missing()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetMealPlanByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException(nameof(MealPlanEntity), 1));

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetMealPlanById(1));
        }

        [Fact]
        public async Task CreateMealPlan_Should_Return_CreatedAtAction()
        {
            var plan = new MealPlanEntity { MealPlanID = 1, PlanName = "Weekly" };
            _mediator.Setup(m => m.Send(It.IsAny<CreateMealPlanCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(plan);

            var result = await _sut.CreateMealPlan(plan);

Assert.IsType<CreatedAtActionResult>(            result.Result);
        }

        [Fact]
        public async Task UpdateMealPlan_Should_Return_BadRequest_On_Id_Mismatch()
        {
            var result = await _sut.UpdateMealPlan(2, new MealPlanEntity { MealPlanID = 1, PlanName = "Weekly" });

Assert.IsType<BadRequestResult>(            result.Result);
            _mediator.Verify(m => m.Send(It.IsAny<UpdateMealPlanCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetMealPlanNutrition_Should_Return_Ok()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetMealPlanNutritionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MealPlanNutritionDto(1, new List<DailyNutritionDto>().AsReadOnly(), new List<MealNutritionDto>().AsReadOnly()));

            var result = await _sut.GetMealPlanNutrition(1);

Assert.IsType<OkObjectResult>(            result.Result);
        }
    }
}
