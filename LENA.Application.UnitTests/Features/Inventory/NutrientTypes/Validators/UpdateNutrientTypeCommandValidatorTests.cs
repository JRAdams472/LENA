using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Inventory.NutrientTypes.Commands;
using LENA.Application.Features.Inventory.NutrientTypes.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.NutrientTypes.Validators
{
    public class UpdateNutrientTypeCommandValidatorTests
    {
        private readonly UpdateNutrientTypeCommandValidator _validator = new UpdateNutrientTypeCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateNutrientTypeCommand(new NutrientType { NutrientName = "Test", UnitOfMeasure = "mg" });
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_NutrientType_Is_Null()
        {
            var command = new UpdateNutrientTypeCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}
