using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void CombineTwoSuccessResults_ReturnsCombinedValue()
    {
        // Arrange
        var result1 = Result.Success("Hello");
        var result2 = Result.Success("World");

        // Act
        var combinedResult = Result.Combine(result1, result2);

        // Assert
        combinedResult.IsSuccess.ShouldBeTrue();
        combinedResult.Value.ShouldBe(["Hello", "World"]);
    }

    [Fact]
    public void CombineSuccessAndFailureResults_ReturnsFailure()
    {
        // Arrange
        var result1 = Result.Success("Hello");
        var result2 = Result.Failure<string>(new Error("Test Error"));

        // Act
        var combinedResult = Result.Combine(result1, result2);

        // Assert
        combinedResult.IsSuccess.ShouldBeFalse();
        combinedResult.Errors.ShouldHaveSingleItem();
        combinedResult.Errors.First().Message.ShouldBe("Test Error");
    }

    [Fact]
    public void CombineTwoFailureResults_ReturnsCombinedErrors()
    {
        // Arrange
        var result1 = Result.Failure<string>(new Error("Error 1"));
        var result2 = Result.Failure<string>(new Error("Error 2"));

        // Act
        var combinedResult = Result.Combine(result1, result2);

        // Assert
        combinedResult.IsSuccess.ShouldBeFalse();
        combinedResult.Errors.Length.ShouldBe(2);
        combinedResult.Errors.Select(e => e.Message).ShouldContain("Error 1");
        combinedResult.Errors.Select(e => e.Message).ShouldContain("Error 2");
    }

    [Fact]
    public void CombineArrayOfSuccessResults_ReturnsCombinedSuccessResult()
    {
        var resultsArray = new[] { Result.Success("World"), Result.Success("!") };
        var combinedResult = resultsArray.Combine();
    }

    [Fact]
    public void CombineArrayOfResultsWithOneFailure_ReturnsCombinedFailureResult()
    {
        var resultsArray = new[] { Result.Success("World"), Result.Failure<string>(new Error("Test Error")) };
        var combinedResult = resultsArray.Combine();

        // Assert
        combinedResult.IsSuccess.ShouldBeFalse();
        combinedResult.Errors.ShouldHaveSingleItem();
        combinedResult.Errors.First().Message.ShouldBe("Test Error");
    }
}
