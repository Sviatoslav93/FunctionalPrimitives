using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Do_ShouldExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "test";

        // Act
        var actualResult = result.Do(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeTrue();
    }

    [Fact]
    public void Do_ShouldNotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = result.Do(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
        ReferenceEquals(result, actualResult).ShouldBeTrue();
    }

    [Fact]
    public void Do_ShouldPassCorrectValue_When_ResultIsSuccess()
    {
        // Arrange
        var capturedValue = string.Empty;
        var expectedValue = "test value";
        var result = Result.Success(expectedValue);

        // Act
        result.Do(value => capturedValue = value);

        // Assert
        capturedValue.ShouldBe(expectedValue);
    }

    [Fact]
    public void Do_ShouldMaintainResultIntegrity_When_ActionThrows()
    {
        // Arrange
        var result = Result.Success("test");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            result.Do(_ => throw new InvalidOperationException("Action failed")));

        exception.Message.ShouldBe("Action failed");
    }

    [Fact]
    public void Do_WithNoArgAction_ShouldExecute_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<int> result = 42;

        // Act
        var actualResult = result.Do(() => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe(42);
        executed.ShouldBeTrue();
    }

    [Fact]
    public void Do_WithNoArgAction_ShouldNotExecute_When_ResultIsFailure()
    {
        // Arrange
        var error = new Error("test error");
        var result = Result.Failure<int>(error);
        var executed = false;

        // Act
        var actualResult = result.Do(() => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
    }

    [Fact]
    public void DoError_ShouldExecute_When_ResultIsFailure()
    {
        // Arrange
        var errors = new[] { new Error("error1"), new Error("error2") };
        var result = Result.Failure<string>(errors);
        IEnumerable<Error>? capturedErrors = null;

        // Act
        var actualResult = result.DoError(errs => capturedErrors = errs);

        // Assert
        capturedErrors.ShouldNotBeNull();
        capturedErrors.ShouldBeSameAs(result.Errors);
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Length.ShouldBe(2);
        ReferenceEquals(result, actualResult).ShouldBeTrue();
    }

    [Fact]
    public void DoError_ShouldNotExecute_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "ok";

        // Act
        var actualResult = result.DoError(_ => executed = true);

        // Assert
        executed.ShouldBeFalse();
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("ok");
        ReferenceEquals(result, actualResult).ShouldBeTrue();
    }
}
