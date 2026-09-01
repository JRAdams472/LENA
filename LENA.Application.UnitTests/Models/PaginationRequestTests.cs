using FluentAssertions;
using LENA.Application.Models;
using Xunit;

namespace LENA.Application.UnitTests.Models;

public class PaginationRequestTests
{
    [Theory]
    [InlineData(0, 0, 1, 25)]
    [InlineData(0, 7, 1, 25)]
    [InlineData(0, 99999, 1, 25)]
    [InlineData(-1, 7, 1, 25)]
    [InlineData(-5, 10, 1, 10)]
    [InlineData(1, 10, 1, 10)]
    [InlineData(2, 25, 2, 25)]
    [InlineData(5, 50, 5, 50)]
    [InlineData(10, 100, 10, 100)]
    public void Clamp_Should_Coerce_PageNumber_And_PageSize(
        int pageNumber,
        int pageSize,
        int expectedPageNumber,
        int expectedPageSize)
    {
        var (clampedPageNumber, clampedPageSize) = PaginationRequest.Clamp(pageNumber, pageSize);

        clampedPageNumber.Should().Be(expectedPageNumber);
        clampedPageSize.Should().Be(expectedPageSize);
    }
}
