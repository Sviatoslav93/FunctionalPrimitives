using FunctionalPrimitives.Extensions.Result;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public async Task BindAsync_ShouldReturnTransformedSuccess_When_ResultIsSuccess_WithTaskValueSelector()
    {
        // Arrange
        Result<int> result = 3;

        // Act
        var actual = await result.BindAsync(x => Task.FromResult(x + 4));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(7);
    }

    [Fact]
    public async Task BindAsync_ShouldReturnResultFromSelector_When_ResultIsSuccess_WithTaskResultSelector()
    {
        // Arrange
        Result<string> result = "hi";

        // Act
        var actual = await result.BindAsync(x => Task.FromResult(Result.Success(x.Length)));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(2);
    }

    [Fact]
    public async Task BindAsync_ShouldPropagateFailure_When_ResultIsFailure_TaskValueSelector()
    {
        // Arrange
        var error = new Error("fail");
        var result = Result.Failure<int>(error);

        // Act
        var actual = await result.BindAsync(x => Task.FromResult(x * 2));

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task BindAsync_ShouldPropagateFailure_When_ResultIsFailure_TaskResultSelector()
    {
        // Arrange
        var errors = new[] { new Error("e1"), new Error("e2") };
        var result = Result.Failure<int>(errors);

        // Act
        var actual = await result.BindAsync(x => Task.FromResult(Result.Success(x * 2)));

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.Count.ShouldBe(2);
        actual.Errors.ShouldBeEquivalentTo(result.Errors);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleSuccess_WithValueSelector()
    {
        // Arrange
        var task = Task.FromResult(Result.Success(10));

        // Act
        var actual = await task.BindAsync(x => x - 3);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(7);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleFailure_WithValueSelector()
    {
        // Arrange
        var error = new Error("bad");
        var task = Task.FromResult(Result.Failure<int>(error));

        // Act
        var actual = await task.BindAsync(x => x - 3);

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleSuccess_WithResultSelector()
    {
        // Arrange
        var task = Task.FromResult(Result.Success("5"));

        // Act
        var actual = await task.BindAsync(x => Result.Success(int.Parse(x)));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(5);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleFailure_WithResultSelector()
    {
        // Arrange
        var errors = new[] { new Error("err") };
        var task = Task.FromResult(Result.Failure<string>(errors));

        // Act
        var actual = await task.BindAsync(x => Result.Success(x.Length));

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldBeEquivalentTo(errors);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleSuccess_WithTaskValueSelector()
    {
        // Arrange
        var task = Task.FromResult(Result.Success(2));

        // Act
        var actual = await task.BindAsync(x => Task.FromResult(x * 5));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleSuccess_WithTaskResultSelector()
    {
        // Arrange
        var task = Task.FromResult(Result.Success("data"));

        // Act
        var actual = await task.BindAsync(x => Task.FromResult(Result.Success(x.Length)));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(4);
    }

    [Fact]
    public async Task BindAsync_TaskSource_ShouldHandleFailure_WithTaskResultSelector()
    {
        // Arrange
        var error = new Error("task fail");
        var task = Task.FromResult(Result.Failure<string>(error));

        // Act
        var actual = await task.BindAsync(x => Task.FromResult(Result.Success(x.Length)));

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task SelectMany_ShouldReturnResultFromSelector_When_ResultIsSuccess_WithTaskResultSelector()
    {
        // Arrange
        Result<int> result = 10;

        // Act
        var actual = await result.SelectMany(x => Task.FromResult(Result.Success(x + 5)));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }

    [Fact]
    public async Task SelectMany_ShouldPropagateFailure_When_ResultIsFailure_WithTaskResultSelector()
    {
        // Arrange
        var error = new Error("source fail");
        var result = Result.Failure<int>(error);
        var wasCalled = false;

        // Act
        var actual = await result.SelectMany(x =>
        {
            wasCalled = true;
            return Task.FromResult(Result.Success(x + 1));
        });

        // Assert
        wasCalled.ShouldBeFalse();
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task SelectMany_WithProjector_ShouldReturnProjectedResult_When_ResultIsSuccess()
    {
        // Arrange
        Result<int> result = 4;

        // Act
        var actual = await result.SelectMany(
            x => Task.FromResult(Result.Success(x * 3)),
            (x, y) => x + y);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(16);
    }

    [Fact]
    public async Task SelectMany_WithProjector_ShouldPropagateIntermediateFailure_When_ResultIsSuccess()
    {
        // Arrange
        Result<int> result = 4;
        var error = new Error("intermediate fail");

        // Act
        var actual = await result.SelectMany(
            _ => Task.FromResult(Result.Failure<int>(error)),
            (x, y) => x + y);

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task SelectMany_TaskSource_ShouldReturnResultFromSelector_When_ResultIsSuccess()
    {
        // Arrange
        var task = Task.FromResult(Result.Success(3));

        // Act
        var actual = await task.SelectMany(x => Task.FromResult(Result.Success(x * 7)));

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(21);
    }

    [Fact]
    public async Task SelectMany_TaskSource_QueryExpression_ShouldWork_WithAsyncBinder()
    {
        // Arrange
        var source = Task.FromResult(Result.Success(2));

        // Act
        var actual = await
            (from x in source
             from y in Task.FromResult(Result.Success(x + 3))
             select x * y);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public async Task SelectMany_ResultSource_QueryExpression_ShouldWork_WithAsyncBinder()
    {
        // Arrange
        Result<int> source = 2;

        // Act
        var actual = await
            (from x in source
             from y in Task.FromResult(Result.Success(x + 3))
             select x * y);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }
}
