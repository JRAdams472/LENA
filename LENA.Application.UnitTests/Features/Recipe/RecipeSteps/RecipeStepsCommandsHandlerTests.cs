using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Recipe.RecipeSteps.Commands;
using LENA.Domain.Entity.Recipe;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Recipe.RecipeSteps
{
    public class RecipeStepsCommandsHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repo = new();

        [Fact]
        public async Task AddRecipeStepCommand_Should_Return_Step_With_Generated_Id()
        {
            var step = new RecipeStep { RecipeID = 1, StepNumber = 1, Instruction = "Boil" };
            _repo.Setup(r => r.AddStepAsync(step, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RecipeStep { RecipeStepID = 42, RecipeID = 1, StepNumber = 1, Instruction = "Boil" });

            var result = await new AddRecipeStepCommandHandler(_repo.Object)
                .Handle(new AddRecipeStepCommand(step), CancellationToken.None);

            result.RecipeStepID.Should().Be(42);
        }

        [Fact]
        public async Task UpdateRecipeStepCommand_Should_Call_UpdateStepAsync()
        {
            var step = new RecipeStep { RecipeStepID = 4, RecipeID = 1, StepNumber = 2, Instruction = "Simmer" };
            _repo.Setup(r => r.UpdateStepAsync(step, It.IsAny<CancellationToken>())).ReturnsAsync(step);

            var result = await new UpdateRecipeStepCommandHandler(_repo.Object)
                .Handle(new UpdateRecipeStepCommand(step), CancellationToken.None);

            result.Should().BeSameAs(step);
            _repo.Verify(r => r.UpdateStepAsync(step, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRecipeStepCommand_Should_Pass_Recipe_Id_For_Ownership_Check()
        {
            await new DeleteRecipeStepCommandHandler(_repo.Object)
                .Handle(new DeleteRecipeStepCommand(4, 1), CancellationToken.None);

            _repo.Verify(r => r.DeleteStepAsync(4, 1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
