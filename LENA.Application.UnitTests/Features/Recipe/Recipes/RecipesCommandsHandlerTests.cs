using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Application.Features.Recipe.Recipes.Commands;

using Moq;

using Xunit;

using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;

namespace LENA.Application.UnitTests.Features.Recipe.Recipes
{
    public class RecipesCommandsHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repo = new();

        [Fact]
        public async Task CreateRecipeCommand_Should_Call_CreateAsync()
        {
            var recipe = new RecipeEntity { RecipeName = "Soup" };
            _repo.Setup(r => r.CreateAsync(recipe, It.IsAny<CancellationToken>())).ReturnsAsync(recipe);

            var result = await new CreateRecipeCommandHandler(_repo.Object)
                .Handle(new CreateRecipeCommand(recipe), CancellationToken.None);

            Assert.Same(recipe, result);
            _repo.Verify(r => r.CreateAsync(recipe, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRecipeCommand_Should_Call_UpdateAsync()
        {
            var recipe = new RecipeEntity { RecipeID = 3, RecipeName = "Soup" };
            _repo.Setup(r => r.UpdateAsync(recipe, It.IsAny<CancellationToken>())).ReturnsAsync(recipe);

            var result = await new UpdateRecipeCommandHandler(_repo.Object)
                .Handle(new UpdateRecipeCommand(recipe), CancellationToken.None);

            Assert.Same(recipe, result);
            _repo.Verify(r => r.UpdateAsync(recipe, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRecipeCommand_Should_Throw_NotFound_When_Missing()
        {
            _repo.Setup(r => r.GetByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync((RecipeEntity?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => new DeleteRecipeCommandHandler(_repo.Object)
                .Handle(new DeleteRecipeCommand(7), CancellationToken.None));

            _repo.Verify(r => r.DeleteAsync(It.IsAny<RecipeEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteRecipeCommand_Should_Delete_When_Found()
        {
            var recipe = new RecipeEntity { RecipeID = 7, RecipeName = "Soup" };
            _repo.Setup(r => r.GetByIdAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(recipe);
            _repo.Setup(r => r.DeleteAsync(recipe, It.IsAny<CancellationToken>())).ReturnsAsync(recipe);

            var result = await new DeleteRecipeCommandHandler(_repo.Object)
                .Handle(new DeleteRecipeCommand(7), CancellationToken.None);

            Assert.Same(recipe, result);
        }
    }
}