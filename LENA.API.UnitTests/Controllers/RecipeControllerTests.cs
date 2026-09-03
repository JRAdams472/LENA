using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.API.Controllers;
using LENA.Application.Features.Recipe.RecipeItems.Commands;
using LENA.Application.Features.Recipe.Recipes.Commands;
using LENA.Application.Features.Recipe.Recipes.Queries;
using LENA.Application.Features.Recipe.RecipeSteps.Commands;
using LENA.Application.Exceptions;
using LENA.Domain.Entity.Recipe;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;

namespace LENA.API.UnitTests.Controllers
{
    public class RecipeControllerTests
    {
        private readonly Mock<IMediator> _mediator = new();
        private readonly RecipeController _sut;

        public RecipeControllerTests() => _sut = new RecipeController(_mediator.Object);

        [Fact]
        public async Task GetRecipes_Should_Return_Ok()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetRecipesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RecipeEntity>());

            var result = await _sut.GetRecipes();

Assert.IsType<OkObjectResult>(            result.Result);
        }

        [Fact]
        public async Task GetRecipeById_Should_Throw_NotFound_When_Missing()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetRecipeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException(nameof(RecipeEntity), 1));

            await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetRecipeById(1));
        }

        [Fact]
        public async Task CreateRecipe_Should_Return_CreatedAtAction()
        {
            var recipe = new RecipeEntity { RecipeID = 1, RecipeName = "Soup" };
            _mediator.Setup(m => m.Send(It.IsAny<CreateRecipeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(recipe);

            var result = await _sut.CreateRecipe(recipe);

Assert.IsType<CreatedAtActionResult>(            result.Result);
        }

        [Fact]
        public async Task UpdateRecipe_Should_Return_BadRequest_On_Id_Mismatch()
        {
            var result = await _sut.UpdateRecipe(2, new RecipeEntity { RecipeID = 1, RecipeName = "Soup" });

Assert.IsType<BadRequestResult>(            result.Result);
            _mediator.Verify(m => m.Send(It.IsAny<UpdateRecipeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddRecipeStep_Should_Not_Accept_Client_Audit_Values()
        {
            RecipeStep? sent = null;
            _mediator.Setup(m => m.Send(It.IsAny<AddRecipeStepCommand>(), It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>((c, _) => sent = ((AddRecipeStepCommand)c).RecipeStep)
                .ReturnsAsync(new RecipeStep { RecipeStepID = 1, RecipeID = 1, StepNumber = 1, Instruction = "Boil" });

            await _sut.AddRecipeStep(1, new RecipeStepRequest(1, "Boil"));

Assert.Empty(            sent!.CreatedBy);
Assert.Equal(default,             sent.CreateDate);
        }

        [Fact]
        public async Task AddRecipeItem_Should_Map_Request_To_Command()
        {
            _mediator.Setup(m => m.Send(It.IsAny<AddOrUpdateRecipeItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RecipeItem { RecipeID = 1, ItemID = 2, Quantity = 3 });

            await _sut.AddRecipeItem(1, new RecipeItemRequest(2, 3, "g", false));

            _mediator.Verify(m => m.Send(
                It.Is<AddOrUpdateRecipeItemCommand>(c =>
                    c.RecipeItem.RecipeID == 1 && c.RecipeItem.ItemID == 2 && c.RecipeItem.Quantity == 3 && c.RecipeItem.UnitOfMeasure == "g"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRecipeStep_Should_Send_Recipe_Scoped_Command()
        {
            await _sut.DeleteRecipeStep(1, 4);

            _mediator.Verify(m => m.Send(
                It.Is<DeleteRecipeStepCommand>(c => c.RecipeStepId == 4 && c.RecipeId == 1),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
