using Result.Abstractions;
using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

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
        actualResult.ShouldBe(result);
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
        var result = Result<string>.Failed(error);

        // Act
        var actualResult = await result.TapAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.ShouldBe(result);
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
        var result = Result<string>.Success(expectedValue);

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
        var task = Task.FromResult(Result<string>.Success("test"));

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
        var task = Task.FromResult(Result<string>.Failed(error));

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
        var task = Task.FromResult(Result<string>.Success("test"));

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
        var task = Task.FromResult(Result<string>.Failed(error));

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
        var result = Result<string>.Success("test");

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
        var result = Result<object>.Success(person);
        var capturedPerson = default(object);

        // Act
        var actualResult = await result.TapAsync(async value =>
        {
            await Task.Delay(1);
            capturedPerson = value;
        });

        // Assert
        actualResult.ShouldBe(result);
        capturedPerson.ShouldBeSameAs(person);
    }

    [Fact]
    public async Task TapAsync_WithMultipleErrors_ShouldNotExecuteAction()
    {
        // Arrange
        var executed = false;
        var errors = new[] { new Error("error1"), new Error("error2") };
        var result = Result<string>.Failed(errors);

        // Act
        var actualResult = await result.TapAsync(async _ =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        actualResult.ShouldBe(result);
        actualResult.IsSuccess.ShouldBeFalse();
        actualResult.Errors.Length.ShouldBe(2);
        executed.ShouldBeFalse();
    }
}
