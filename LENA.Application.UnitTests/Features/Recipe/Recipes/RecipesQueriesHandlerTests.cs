using LENA.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LENA.Application.Contracts.Persistence;
using LENA.Application.Features.Recipe.Recipes.Queries;
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

            result.Should().BeEquivalentTo(recipes);
        }

        [Fact]
        public async Task GetRecipesPagedQuery_Should_Return_PagedResult_And_Pass_Through_Page_And_Size()
        {
            IReadOnlyList<RecipeEntity> recipes = new List<RecipeEntity> { new() { RecipeName = "Soup" } };
            _repo.Setup(r => r.ListPagedAsync(2, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedResult<RecipeEntity> { Items = recipes, PageNumber = 2, PageSize = 10, TotalCount = 1 });

            var result = await new GetRecipesPagedQueryHandler(_repo.Object)
                .Handle(new GetRecipesPagedQuery(2, 10), CancellationToken.None);

            result.Items.Should().BeEquivalentTo(recipes);
            result.PageNumber.Should().Be(2);
            result.PageSize.Should().Be(10);
            _repo.Verify(r => r.ListPagedAsync(2, 10, It.IsAny<CancellationToken>()), Times.Once);
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

            result!.RecipeItems.Should().HaveCount(1);
            result.RecipeSteps.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetRecipeByIdQuery_Should_Return_Null_When_Missing()
        {
            _repo.Setup(r => r.GetByIdAsync(9, It.IsAny<CancellationToken>())).ReturnsAsync((RecipeEntity?)null);

            var result = await new GetRecipeByIdQueryHandler(_repo.Object)
                .Handle(new GetRecipeByIdQuery(9), CancellationToken.None);

            result.Should().BeNull();
            _repo.Verify(r => r.GetItemsByRecipeIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
