using FunctionalPrimitives.Extensions.Result;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public async Task TapAsync_ShouldExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "test";

        // Act
        var actualResult = await result.TapAsync(async _ =>
        {
            await Task.Delay(0);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeTrue();
    }

    [Fact]
    public async Task TapAsync_ShouldNotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = await result.TapAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task TapAsync_ShouldPassCorrectValue_When_ResultIsSuccess()
    {
        // Arrange
        var capturedValue = string.Empty;
        var expectedValue = "test value";
        var result = Result.Success(expectedValue);

        // Act
        await result.TapAsync(async value =>
        {
            await Task.Delay(1);
            capturedValue = value;
        });

        // Assert
        capturedValue.ShouldBe(expectedValue);
    }

    [Fact]
    public async Task TapAsync_WithTaskResult_ShouldExecuteSyncAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        var task = Task.FromResult(Result.Success("test"));

        // Act
        var actualResult = await task.TapAsync(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeTrue();
    }

    [Fact]
    public async Task TapAsync_WithTaskResult_ShouldNotExecuteSyncAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var task = Task.FromResult(Result.Failure<string>(error));

        // Act
        var actualResult = await task.TapAsync(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task TapAsync_WithTaskResult_ShouldExecuteAsyncAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        var task = Task.FromResult(Result.Success("test"));

        // Act
        var actualResult = await task.TapAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeTrue();
    }

    [Fact]
    public async Task TapAsync_WithTaskResult_ShouldNotExecuteAsyncAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var task = Task.FromResult(Result.Failure<string>(error));

        // Act
        var actualResult = await task.TapAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task TapAsync_ShouldMaintainResultIntegrity_When_ActionThrows()
    {
        // Arrange
        var result = Result.Success("test");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            result.TapAsync(async _ =>
            {
                await Task.Delay(1);
                throw new InvalidOperationException("Action failed");
            }));

        exception.Message.ShouldBe("Action failed");
    }

    [Fact]
    public async Task TapAsync_WithComplexValue_ShouldExecuteCorrectly()
    {
        // Arrange
        var person = new { Name = "John", Age = 30 };
        var result = Result.Success(person);
        var capturedPerson = default(object);

        // Act
        var actualResult = await result.TapAsync(async value =>
        {
            await Task.Delay(1);
            capturedPerson = value;
        });

        // Assert
        capturedPerson.ShouldBeSameAs(person);
    }

    [Fact]
    public async Task TapAsync_WithMultipleErrors_ShouldNotExecuteAction()
    {
        // Arrange
        var executed = false;
        var errors = new[] { new Error("error1"), new Error("error2") };
        var result = Failure<string>(errors);

        // Act
        var actualResult = await result.TapAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Count.ShouldBe(2);
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task TapErrorAsync_ShouldExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var result = Failure<string>(error);

        // Act
        var actualResult = await result.TapErrorAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeTrue();
    }

    [Fact]
    public async Task TapErrorAsync_ShouldNotExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "test";

        // Act
        var actualResult = await result.TapErrorAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task TapErrorAsync_ShouldPassCorrectErrors_When_ResultIsFailure()
    {
        // Arrange
        IEnumerable<Error> capturedErrors = [];
        var expectedErrors = new[] { new Error("error1"), new Error("error2") };
        var result = Result.Failure<string>(expectedErrors);

        // Act
        await result.TapErrorAsync(async errors =>
        {
            await Task.Delay(1);
            capturedErrors = errors;
        });

        // Assert
        capturedErrors.ShouldNotBeNull();
        capturedErrors.ShouldBe(expectedErrors);
    }

    [Fact]
    public async Task TapErrorAsync_WithTaskResult_ShouldExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var task = Task.FromResult(Failure<string>(error));

        // Act
        var actualResult = await task.TapErrorAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeTrue();
    }

    [Fact]
    public async Task TapErrorAsync_WithTaskResult_ShouldNotExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        var task = Task.FromResult(Success("test"));

        // Act
        var actualResult = await task.TapErrorAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task TapErrorAsync_ShouldMaintainResultIntegrity_When_ActionThrows()
    {
        // Arrange
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            result.TapErrorAsync(async _ =>
            {
                await Task.Delay(1);
                throw new InvalidOperationException("Action failed");
            }));

        exception.Message.ShouldBe("Action failed");
    }

    [Fact]
    public async Task TapErrorAsync_WithMultipleErrors_ShouldExecuteActionAndPassAllErrors()
    {
        // Arrange
        var executed = false;
        IEnumerable<Error> capturedErrors = [];
        var errors = new[] { new Error("error1"), new Error("error2"), new Error("error3") };
        var result = Failure<string>(errors);

        // Act
        var actualResult = await result.TapErrorAsync(async errs =>
        {
            await Task.Delay(1);
            capturedErrors = errs;
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Count.ShouldBe(3);
        executed.ShouldBeTrue();
        capturedErrors.ShouldNotBeNull();
        capturedErrors.Count().ShouldBe(3);
    }

    [Fact]
    public async Task TapErrorAsync_ShouldReturnSameResult_When_ResultIsFailure()
    {
        // Arrange
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = await result.TapErrorAsync(async _ => await Task.Delay(1));

        // Assert
        actualResult.ShouldBe(result);
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldBe(result.Errors);
    }

    [Fact]
    public async Task TapErrorAsync_ShouldReturnSameResult_When_ResultIsSuccess()
    {
        // Arrange
        Result<string> result = "test";

        // Act
        var actualResult = await result.TapErrorAsync(async _ => await Task.Delay(1));

        // Assert
        actualResult.ShouldBe(result);
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
    }
}
