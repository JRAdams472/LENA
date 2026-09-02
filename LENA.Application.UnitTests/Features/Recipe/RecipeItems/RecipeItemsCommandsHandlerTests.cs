using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Recipe.RecipeItems.Commands;
using LENA.Domain.Entity.Recipe;
using Moq;
using Xunit;

namespace LENA.Application.UnitTests.Features.Recipe.RecipeItems
{
    public class RecipeItemsCommandsHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repo = new();

        [Fact]
        public async Task AddOrUpdateRecipeItemCommand_Should_Return_Persisted_Row()
        {
            var input = new RecipeItem { RecipeID = 1, ItemID = 2, Quantity = 3 };
            var persisted = new RecipeItem { RecipeID = 1, ItemID = 2, Quantity = 3, UnitOfMeasure = "g" };
            _repo.Setup(r => r.AddOrUpdateRecipeItemAsync(input, It.IsAny<CancellationToken>())).ReturnsAsync(persisted);

            var result = await new AddOrUpdateRecipeItemCommandHandler(_repo.Object)
                .Handle(new AddOrUpdateRecipeItemCommand(input), CancellationToken.None);

Assert.Same(persisted,             result);
        }

        [Fact]
        public async Task RemoveRecipeItemCommand_Should_Call_RemoveRecipeItemAsync()
        {
            await new RemoveRecipeItemCommandHandler(_repo.Object)
                .Handle(new RemoveRecipeItemCommand(1, 2), CancellationToken.None);

            _repo.Verify(r => r.RemoveRecipeItemAsync(1, 2, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
