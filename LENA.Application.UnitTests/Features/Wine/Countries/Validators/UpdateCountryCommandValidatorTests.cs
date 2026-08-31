using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Wine.Countries.Commands;
using LENA.Application.Features.Wine.Countries.Validators;
using LENA.Domain.Entity.Wine;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Countries.Validators
{
    public class UpdateCountryCommandValidatorTests
    {
        private readonly UpdateCountryCommandValidator _validator = new UpdateCountryCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new UpdateCountryCommand(new Country { CountryName = "Test", ISOCode = "XX" });
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_Country_Is_Null()
        {
            var command = new UpdateCountryCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}
