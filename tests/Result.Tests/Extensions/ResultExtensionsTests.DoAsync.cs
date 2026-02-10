using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public async Task DoAsync_ShouldExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "test";

        // Act
        var actualResult = await result.DoAsync(async _ =>
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
    public async Task DoAsync_ShouldNotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = await result.DoAsync(async _ =>
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
    public async Task DoAsync_ShouldPassCorrectValue_When_ResultIsSuccess()
    {
        // Arrange
        var capturedValue = string.Empty;
        var expectedValue = "test value";
        var result = Result.Success(expectedValue);

        // Act
        await result.DoAsync(async value =>
        {
            await Task.Delay(1);
            capturedValue = value;
        });

        // Assert
        capturedValue.ShouldBe(expectedValue);
    }

    [Fact]
    public async Task DoAsync_WithTaskResult_ShouldExecuteSyncAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        var task = Task.FromResult(Result.Success("test"));

        // Act
        var actualResult = await task.DoAsync(_ => executed = true);

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
        var actualResult = await task.DoAsync(_ => executed = true);

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldHaveSingleItem();
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task DoAsync_WithTaskResult_ShouldExecuteAsyncAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        var task = Task.FromResult(Result.Success("test"));

        // Act
        var actualResult = await task.DoAsync(async _ =>
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
        var actualResult = await task.DoAsync(async _ =>
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
    public async Task DoAsync_ShouldMaintainResultIntegrity_When_ActionThrows()
    {
        // Arrange
        var result = Result.Success("test");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            result.DoAsync(async _ =>
            {
                await Task.Delay(1);
                throw new InvalidOperationException("Action failed");
            }));

        exception.Message.ShouldBe("Action failed");
    }

    [Fact]
    public async Task DoAsync_WithComplexValue_ShouldExecuteCorrectly()
    {
        // Arrange
        var person = new { Name = "John", Age = 30 };
        var result = Result.Success(person);
        var capturedPerson = default(object);

        // Act
        var actualResult = await result.DoAsync(async value =>
        {
            await Task.Delay(1);
            capturedPerson = value;
        });

        // Assert
        capturedPerson.ShouldBeSameAs(person);
    }

    [Fact]
    public async Task DoAsync_WithMultipleErrors_ShouldNotExecuteAction()
    {
        // Arrange
        var executed = false;
        var errors = new[] { new Error("error1"), new Error("error2") };
        var result = Result.Failure<string>(errors);

        // Act
        var actualResult = await result.DoAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Length.ShouldBe(2);
        executed.ShouldBeFalse();
    }

    [Fact]
    public async Task DoErrorAsync_ShouldExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = await result.DoErrorAsync(async _ =>
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
    public async Task DoErrorAsync_ShouldNotExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        Result<string> result = "test";

        // Act
        var actualResult = await result.DoErrorAsync(async _ =>
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
    public async Task DoErrorAsync_ShouldPassCorrectErrors_When_ResultIsFailure()
    {
        // Arrange
        IEnumerable<Error> capturedErrors = [];
        var expectedErrors = new[] { new Error("error1"), new Error("error2") };
        var result = Result.Failure<string>(expectedErrors);

        // Act
        await result.DoErrorAsync(async errors =>
        {
            await Task.Delay(1);
            capturedErrors = errors;
        });

        // Assert
        capturedErrors.ShouldNotBeNull();
        capturedErrors.ShouldBe(expectedErrors);
    }

    [Fact]
    public async Task DoErrorAsync_WithTaskResult_ShouldExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var executed = false;
        var error = new Error("test error");
        var task = Task.FromResult(Result.Failure<string>(error));

        // Act
        var actualResult = await task.DoErrorAsync(async _ =>
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
    public async Task DoErrorAsync_WithTaskResult_ShouldNotExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var executed = false;
        var task = Task.FromResult(Result.Success("test"));

        // Act
        var actualResult = await task.DoErrorAsync(async _ =>
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
    public async Task DoErrorAsync_ShouldMaintainResultIntegrity_When_ActionThrows()
    {
        // Arrange
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            result.DoErrorAsync(async _ =>
            {
                await Task.Delay(1);
                throw new InvalidOperationException("Action failed");
            }));

        exception.Message.ShouldBe("Action failed");
    }

    [Fact]
    public async Task DoErrorAsync_WithMultipleErrors_ShouldExecuteActionAndPassAllErrors()
    {
        // Arrange
        var executed = false;
        IEnumerable<Error> capturedErrors = [];
        var errors = new[] { new Error("error1"), new Error("error2"), new Error("error3") };
        var result = Result.Failure<string>(errors);

        // Act
        var actualResult = await result.DoErrorAsync(async errs =>
        {
            await Task.Delay(1);
            capturedErrors = errs;
            executed = true;
        });

        // Assert
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Length.ShouldBe(3);
        executed.ShouldBeTrue();
        capturedErrors.ShouldNotBeNull();
        capturedErrors.Count().ShouldBe(3);
    }

    [Fact]
    public async Task DoErrorAsync_ShouldReturnSameResult_When_ResultIsFailure()
    {
        // Arrange
        var error = new Error("test error");
        var result = Result.Failure<string>(error);

        // Act
        var actualResult = await result.DoErrorAsync(async _ => await Task.Delay(1));

        // Assert
        actualResult.ShouldBe(result);
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.ShouldBe(result.Errors);
    }

    [Fact]
    public async Task DoErrorAsync_ShouldReturnSameResult_When_ResultIsSuccess()
    {
        // Arrange
        Result<string> result = "test";

        // Act
        var actualResult = await result.DoErrorAsync(async _ => await Task.Delay(1));

        // Assert
        actualResult.ShouldBe(result);
        actualResult.IsSuccess.ShouldBeTrue();
        actualResult.Value.ShouldBe("test");
    }
}
