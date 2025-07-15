using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void CombineTwoSuccessResults_ReturnsCombinedValue()
    {
        // Arrange
        var result1 = Result<string>.Success("Hello");
        var result2 = Result<string>.Success("World");

        // Act
        var combinedResult = Result<string>.Combine(result1, result2);

        // Assert
        combinedResult.IsSuccess.ShouldBeTrue();
        combinedResult.Value.ShouldBe(["Hello", "World"]);
    }
}
