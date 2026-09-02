using FluentValidation;
using LENA.Application.Features.Inventory.NutrientTypes.Commands;
using LENA.Application.Features.Inventory.NutrientTypes.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.NutrientTypes.Validators
{
    public class CreateNutrientTypeCommandValidatorTests
    {
        private readonly CreateNutrientTypeCommandValidator _validator = new CreateNutrientTypeCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateNutrientTypeCommand(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var result = _validator.Validate(command);
Assert.True(            result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_NutrientType_Is_Null()
        {
            var command = new CreateNutrientTypeCommand(null!);
            var result = _validator.Validate(command);
Assert.False(            result.IsValid);
        }
    }
}
