using FunctionalPrimitives.Extensions.Result;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Tap_ShouldExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "test";

        // Act
        var actualResult = result.Tap(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeTrue();
    }

    [Fact]
    public void Tap_ShouldNotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = result.Tap(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
        ReferenceEquals(result, actualResult).ShouldBeTrue();
    }

    [Fact]
    public void Tap_ShouldPassCorrectValue_When_ResultIsSuccess()
    {
        // Arrange
        var capturedValue = string.Empty;
        var expectedValue = "test value";
        var result = Result.Success(expectedValue);

        // Act
        result.Tap(value => capturedValue = value);

        // Assert
        capturedValue.ShouldBe(expectedValue);
    }

    [Fact]
    public void Tap_ShouldMaintainResultIntegrity_When_ActionThrows()
    {
        // Arrange
        var result = Result.Success("test");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            result.Tap(_ => throw new InvalidOperationException("Action failed")));

        exception.Message.ShouldBe("Action failed");
    }

    [Fact]
    public void Tap_WithNoArgAction_ShouldExecute_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<int> result = 42;

        // Act
        var actualResult = result.Tap(() => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe(42);
        executed.ShouldBeTrue();
    }

    [Fact]
    public void Tap_WithNoArgAction_ShouldNotExecute_When_ResultIsFailure()
    {
        // Arrange
        var error = new Error("test error");
        var result = Result.Failure<int>(error);
        var executed = false;

        // Act
        var actualResult = result.Tap(() => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
    }

    [Fact]
    public void TapError_ShouldExecute_When_ResultIsFailure()
    {
        // Arrange
        var errors = new[] { new Error("error1"), new Error("error2") };
        var result = Result.Failure<string>(errors);
        IEnumerable<Error>? capturedErrors = null;

        // Act
        var actualResult = result.TapError(errs => capturedErrors = errs);

        // Assert
        capturedErrors.ShouldNotBeNull();
        capturedErrors.ShouldBeSameAs(result.Errors);
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Count.ShouldBe(2);
        ReferenceEquals(result, actualResult).ShouldBeTrue();
    }

    [Fact]
    public void TapError_ShouldNotExecute_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "ok";

        // Act
        var actualResult = result.TapError(_ => executed = true);

        // Assert
        executed.ShouldBeFalse();
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("ok");
        ReferenceEquals(result, actualResult).ShouldBeTrue();
    }
}
