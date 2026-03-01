using FunctionalPrimitives.ResultExtensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Bind_ShouldReturnTransformedSuccess_When_ResultIsSuccess_WithValueSelector()
    {
        // Arrange
        Result<int> result = 2;

        // Act
        var actual = result.Bind(x => x * 10);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(20);
    }

    [Fact]
    public void Bind_ShouldReturnResultFromSelector_When_ResultIsSuccess_WithResultSelector()
    {
        // Arrange
        Result<int> result = 5;

        // Act
        var actual = result.Bind(x => Result.Success(x.ToString()));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe("5");
    }

    [Fact]
    public void Bind_ShouldPropagateFailure_When_ResultIsFailure_ValueSelector()
    {
        // Arrange
        var error = new Error("bind failed");
        var result = Result.Failure<int>(error);

        // Act
        var actual = result.Bind(x => x * 2);

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public void Bind_ShouldPropagateFailure_When_ResultIsFailure_ResultSelector()
    {
        // Arrange
        var errors = new[] { new Error("e1"), new Error("e2") };
        var result = Result.Failure<int>(errors);

        // Act
        var actual = result.Bind(x => Result.Success(x * 3));

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.Count.ShouldBe(2);
        actual.Errors.ShouldBeEquivalentTo(result.Errors);
    }

    [Fact]
    public void Bind_ShouldUseValueFromResult_When_ChainingMultipleSuccesses()
    {
        // Arrange
        var result = Result.Success("3");

        // Act
        var actual = result
            .Bind(x => int.Parse(x))
            .Bind(x => x + 7);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public void Bind_ShouldStopAtFirstFailure_When_Chaining()
    {
        // Arrange
        var result = Result.Success("abc");

        // Act
        var actual = result
            .Bind(x => int.TryParse(x, out var value) ? Result.Success(value) : Result.Failure<int>(new Error("parse")))
            .Bind(x => x + 1);

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
    }
}
