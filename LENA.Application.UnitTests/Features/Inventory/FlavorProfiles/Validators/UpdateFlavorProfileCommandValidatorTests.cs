using FluentValidation;

using LENA.Application.Features.Inventory.FlavorProfiles.Commands;
using LENA.Application.Features.Inventory.FlavorProfiles.Validators;
using LENA.Domain.Entity.Inventory;

using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FlavorProfiles.Validators
{
    public class UpdateFlavorProfileCommandValidatorTests
    {
        private readonly UpdateFlavorProfileCommandValidator _validator = new UpdateFlavorProfileCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateFlavorProfileCommand(new FlavorProfile { FlavorName = "Test" });
            var result = _validator.Validate(command);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Be_Invalid_When_FlavorProfile_Is_Null()
        {
            var command = new UpdateFlavorProfileCommand(null!);
            var result = _validator.Validate(command);
            Assert.False(result.IsValid);
        }
    }
}