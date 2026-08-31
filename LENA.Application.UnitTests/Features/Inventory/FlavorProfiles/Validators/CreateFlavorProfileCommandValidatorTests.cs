using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Inventory.FlavorProfiles.Commands;
using LENA.Application.Features.Inventory.FlavorProfiles.Validators;
using LENA.Domain.Entity.Inventory;
using Xunit;

namespace LENA.Application.UnitTests.Features.Inventory.FlavorProfiles.Validators
{
    public class CreateFlavorProfileCommandValidatorTests
    {
        private readonly CreateFlavorProfileCommandValidator _validator = new CreateFlavorProfileCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateFlavorProfileCommand(new FlavorProfile { FlavorName = "Test" });
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_FlavorProfile_Is_Null()
        {
            var command = new CreateFlavorProfileCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}
