using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Contracts.Persistence;
using LENA.Application.Exceptions;
using LENA.Application.Features.Recipe.Recipes.Queries;
using LENA.Application.Models;
using LENA.Domain.Entity.Recipe;

using Moq;

using Xunit;

using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;

namespace LENA.Application.UnitTests.Features.Recipe.Recipes
{
    public class RecipesQueriesHandlerTests
    {
        private readonly Mock<IRecipeRepository> _repo = new();

        [Fact]
        public async Task GetRecipesQuery_Should_Return_All_Recipes()
        {
            IReadOnlyList<RecipeEntity> recipes = new List<RecipeEntity> { new() { RecipeName = "Soup" } };
            _repo.Setup(r => r.ListAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(recipes);

            var result = await new GetRecipesQueryHandler(_repo.Object)
                .Handle(new GetRecipesQuery(), CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(recipes[0].RecipeName, result[0].RecipeName);
        }

        [Fact]
        public async Task GetRecipesPagedQuery_Should_Return_PagedResult_And_Pass_Through_Page_And_Size()
        {
            IReadOnlyList<RecipeEntity> recipes = new List<RecipeEntity> { new() { RecipeName = "Soup" } };
            _repo.Setup(r => r.ListPagedAsync(2, 10, It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<RecipeEntity> { Items = recipes, PageNumber = 2, PageSize = 10, TotalCount = 1 });

            var result = await new GetRecipesPagedQueryHandler(_repo.Object)
                .Handle(new GetRecipesPagedQuery(2, 10, null, false), CancellationToken.None);

            Assert.Single(result.Items);
            Assert.Equal(recipes[0].RecipeName, result.Items[0].RecipeName);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            _repo.Verify(r => r.ListPagedAsync(2, 10, It.IsAny<string?>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetRecipeByIdQuery_Should_Populate_Items_And_Steps()
        {
            var recipe = new RecipeEntity { RecipeID = 1, RecipeName = "Soup" };
            _repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(recipe);
            _repo.Setup(r => r.GetItemsByRecipeIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RecipeItem> { new() { RecipeID = 1, ItemID = 2, Quantity = 1 } });
            _repo.Setup(r => r.GetStepsByRecipeIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RecipeStep> { new() { RecipeStepID = 5, RecipeID = 1, StepNumber = 1, Instruction = "Boil" } });

            var result = await new GetRecipeByIdQueryHandler(_repo.Object)
                .Handle(new GetRecipeByIdQuery(1), CancellationToken.None);

            Assert.Single(result!.RecipeItems!);
            Assert.Single(result.RecipeSteps!);
        }

        [Fact]
        public async Task GetRecipeByIdQuery_Should_Throw_NotFound_When_Missing()
        {
            _repo.Setup(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync((RecipeEntity?)null);

            await Assert.ThrowsAsync<NotFoundException>(() => new GetRecipeByIdQueryHandler(_repo.Object)
                .Handle(new GetRecipeByIdQuery(9), CancellationToken.None));

            _repo.Verify(r => r.GetItemsByRecipeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}