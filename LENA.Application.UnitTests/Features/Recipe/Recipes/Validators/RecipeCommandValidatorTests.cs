using FluentAssertions;
using LENA.Application.Features.Recipe.RecipeItems.Commands;
using LENA.Application.Features.Recipe.RecipeItems.Validators;
using LENA.Application.Features.Recipe.Recipes.Commands;
using LENA.Application.Features.Recipe.Recipes.Validators;
using LENA.Application.Features.Recipe.RecipeSteps.Commands;
using LENA.Application.Features.Recipe.RecipeSteps.Validators;
using LENA.Domain.Entity.Recipe;
using Xunit;
using RecipeEntity = LENA.Domain.Entity.Recipe.Recipe;

namespace LENA.Application.UnitTests.Features.Recipe.Recipes.Validators
{
    public class RecipeCommandValidatorTests
    {
        [Fact]
        public void CreateRecipe_Is_Valid_With_Name_And_Servings()
        {
            var command = new CreateRecipeCommand(new RecipeEntity { RecipeName = "Soup", Servings = 2 });
            new CreateRecipeCommandValidator().Validate(command).IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateRecipe_Is_Invalid_Without_Name()
        {
            var command = new CreateRecipeCommand(new RecipeEntity { RecipeName = string.Empty, Servings = 2 });
            new CreateRecipeCommandValidator().Validate(command).IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateRecipe_Is_Invalid_Without_Id()
        {
            var command = new UpdateRecipeCommand(new RecipeEntity { RecipeName = "Soup", Servings = 2 });
            new UpdateRecipeCommandValidator().Validate(command).IsValid.Should().BeFalse();
        }

        [Fact]
        public void AddRecipeStep_Is_Invalid_Without_Instruction()
        {
            var command = new AddRecipeStepCommand(new RecipeStep { RecipeID = 1, StepNumber = 1, Instruction = string.Empty });
            new AddRecipeStepCommandValidator().Validate(command).IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateRecipeStep_Is_Valid_With_Complete_Step()
        {
            var command = new UpdateRecipeStepCommand(new RecipeStep { RecipeStepID = 1, RecipeID = 1, StepNumber = 1, Instruction = "Boil" });
            new UpdateRecipeStepCommandValidator().Validate(command).IsValid.Should().BeTrue();
        }

        [Fact]
        public void DeleteRecipeStep_Is_Invalid_Without_Recipe_Id()
        {
            var command = new DeleteRecipeStepCommand(1, 0);
            new DeleteRecipeStepCommandValidator().Validate(command).IsValid.Should().BeFalse();
        }

        [Fact]
        public void AddRecipeItem_Is_Invalid_With_Zero_Quantity()
        {
            var command = new AddOrUpdateRecipeItemCommand(new RecipeItem { RecipeID = 1, ItemID = 1, Quantity = 0 });
            new AddOrUpdateRecipeItemCommandValidator().Validate(command).IsValid.Should().BeFalse();
        }
    }
}
