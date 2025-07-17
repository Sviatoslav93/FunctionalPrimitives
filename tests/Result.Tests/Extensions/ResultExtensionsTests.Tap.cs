using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Tap_WhenResultIsSuccess_ShouldExecuteAction()
    {
        // Arrange
        var value = 42;
        var result = Result.Success(value);
        var actionExecuted = false;
        int capturedValue = 0;

        // Act
        var returnedResult = result.Tap(v =>
        {
            actionExecuted = true;
            capturedValue = v;
        });

        // Assert
        actionExecuted.ShouldBeTrue();
        capturedValue.ShouldBe(value);
        returnedResult.ShouldBe(result);
    }

    [Fact]
    public void Tap_WhenResultIsFailed_ShouldNotExecuteAction()
    {
        // Arrange
        Result<int> result = new Error("test error");
        var actionExecuted = false;

        // Act
        var returnedResult = result.Tap(_ => actionExecuted = true);

        // Assert
        actionExecuted.ShouldBeFalse();
        returnedResult.ShouldBe(result);
    }

    [Fact]
    public void TapError_WhenResultIsFailed_ShouldExecuteAction()
    {
        // Arrange
        var errors = new[]
        {
            new Error("error one"),
            new Error("error two"),
        };
        var result = Result.Failed<int>(errors);
        var actionExecuted = false;
        IEnumerable<Error> capturedErrors = null!;

        // Act
        var returnedResult = result.TapError(e =>
        {
            actionExecuted = true;
            capturedErrors = e;
        });

        // Assert
        actionExecuted.ShouldBeTrue();
        capturedErrors.ShouldBe(errors);
        returnedResult.ShouldBe(result);
    }

    [Fact]
    public void TapError_WhenResultIsSuccess_ShouldNotExecuteAction()
    {
        // Arrange
        var result = Result.Success(42);
        var actionExecuted = false;

        // Act
        var returnedResult = result.TapError(_ => actionExecuted = true);

        // Assert
        actionExecuted.ShouldBeFalse();
        returnedResult.ShouldBe(result);
    }

    [Fact]
    public void Tap_ShouldAllowMethodChaining()
    {
        // Arrange
        var result = Result.Success(42);
        var callOrder = new List<string>();

        // Act
        var finalResult = result
            .Tap(v => callOrder.Add($"Tap1:{v}"))
            .Tap(v => callOrder.Add($"Tap2:{v}"))
            .TapError(e => callOrder.Add("TapError"));

        // Assert
        callOrder.ShouldBe(new[] { "Tap1:42", "Tap2:42" });
        finalResult.ShouldBe(result);
    }
}
