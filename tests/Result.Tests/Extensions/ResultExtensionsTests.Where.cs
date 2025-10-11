using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void WhereOnSuccessResult_WithPredicate_ReturnsFilteredResult()
    {
        // Arrange
        var result = Result.Success(5);
        static bool IsValueGreaterThanThree(int x) => x > 3;
        var error = new Error("Value is not greater than 3"); // Act
        var filteredResult = result.Where(IsValueGreaterThanThree, error);

        // Assert
        filteredResult.IsSuccess.ShouldBeTrue();
        filteredResult.Value.ShouldBe(5);
    }

    [Fact]
    public void WhereOnSuccessResult_WithPredicateThatFails_ReturnsFailedResult()
    {
        // Arrange
        var result = Result.Success(2);
        static bool IsValueGreaterThanThree(int x) => x > 3;
        var error = new Error("Value is not greater than 3");

        // Act
        var filteredResult = result.Where(IsValueGreaterThanThree, error);

        // Assert
        filteredResult.IsSuccess.ShouldBeFalse();
        filteredResult.Errors[0].ShouldBe(error);
    }

    [Fact]
    public void WhereOnFailedResult_WithPredicate_ReturnsFailedResult()
    {
        // Arrange
        var initialError = new Error("Initial failure");
        var result = Result.Failure<int>(initialError);
        static bool IsValueGreaterThanThree(int x) => x > 3;
        var error = new Error("Value is not greater than 3");

        // Act
        var filteredResult = result.Where(IsValueGreaterThanThree, error);

        // Assert
        filteredResult.IsSuccess.ShouldBeFalse();
        filteredResult.Errors[0].ShouldBe(initialError);
    }

    [Fact]
    public void WhereOnSuccessResult_WithPredicateAndErrorFactory_ReturnsFilteredFactoryResult()
    {
        // Arrange
        var result = Result.Success(5);
        static bool IsValueGreaterThanThree(int x) => x > 3;
        static Error CreateError(int x) => new($"Value {x} is not greater than 3");

        // Act
        var filteredResult = result.Where(IsValueGreaterThanThree, CreateError);

        // Assert
        filteredResult.IsSuccess.ShouldBeTrue();
        filteredResult.Value.ShouldBe(5);
    }
}
