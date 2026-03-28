using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Results.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void ToResult_ShouldWrapValueInResult()
    {
        // Arrange
        var value = 42;

        // Act
        var result = value.ToResult();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void ToResult_NullableClass_WithNonNull_ReturnsSuccess()
    {
        const string? value = "hello";

        var result = value.ToResult(new Error("was null"));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("hello");
    }

    [Fact]
    public void ToResult_NullableClass_WithNull_ReturnsFailureWithProvidedError()
    {
        string? value = null;
        var error = new Error("was null");

        var result = value.ToResult(error);

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldContain(error);
    }

    [Fact]
    public void Ignore_Success_ReturnsSuccessResultOfUnit()
    {
        var result = Success("hello");

        var ignored = result.Ignore();

        ignored.IsSuccess.ShouldBeTrue();
        ignored.Value.ShouldBe(Unit.Value);
    }

    [Fact]
    public void Ignore_Failure_PropagatesErrors()
    {
        var error = new Error("something failed");
        var result = Failure<string>(error);

        var ignored = result.Ignore();

        ignored.IsSuccess.ShouldBeFalse();
        ignored.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task IgnoreAsync_Success_ReturnsSuccessResultOfUnit()
    {
        var task = Task.FromResult(Success("hello"));

        var ignored = await task.IgnoreAsync();

        ignored.IsSuccess.ShouldBeTrue();
        ignored.Value.ShouldBe(Unit.Value);
    }

    [Fact]
    public async Task IgnoreAsync_Failure_PropagatesErrors()
    {
        var error = new Error("async fail");
        var task = Task.FromResult(Failure<string>(error));

        var ignored = await task.IgnoreAsync();

        ignored.IsSuccess.ShouldBeFalse();
        ignored.Errors.ShouldContain(error);
    }

    [Fact]
    public void ToOption_Success_ReturnsSome()
    {
        var result = Success("hello");

        var option = result.ToOption();

        option.HasValue.ShouldBeTrue();
        option.Value.ShouldBe("hello");
    }

    [Fact]
    public void ToOption_Failure_ReturnsNone()
    {
        var result = Failure<string>(new Error("failed"));
        var option = result.ToOption();

        option.HasValue.ShouldBeFalse();
    }
}
