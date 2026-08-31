using FluentAssertions;
using FluentValidation;
using LENA.Application.Features.Wine.Regions.Commands;
using LENA.Application.Features.Wine.Regions.Validators;
using LENA.Domain.Entity.Wine;
using Xunit;

namespace LENA.Application.UnitTests.Features.Wine.Regions.Validators
{
    public class CreateRegionCommandValidatorTests
    {
        private readonly CreateRegionCommandValidator _validator = new CreateRegionCommandValidator();

        [Fact]
        public void Should_Be_Valid_With_Correct_Input()
        {
            var command = new CreateRegionCommand(new Region { RegionName = "Test" });
            var result = _validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_Invalid_When_Region_Is_Null()
        {
            var command = new CreateRegionCommand(null!);
            var result = _validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}
