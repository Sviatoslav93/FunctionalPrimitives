using FunctionalPrimitives.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public async Task Select_TaskSource_ShouldProjectValue_When_ResultIsSuccess()
    {
        // Arrange
        var task = Task.FromResult(Result.Success(21));

        // Act
        var actual = await task.Select(x => x * 2);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(42);
    }

    [Fact]
    public async Task Select_TaskSource_ShouldPropagateFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = new Error("map failed");
        var task = Task.FromResult(Result.Failure<int>(error));

        // Act
        var actual = await task.Select(x => x * 2);

        // Assert
        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldHaveSingleItem();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task QueryExpression_TaskSource_ShouldCompileAndProject_When_SingleFromClause()
    {
        // Arrange
        var task = Task.FromResult(Result.Success(10));

        // Act
        var actual = await (from value in task
                            select value + 5);

        // Assert
        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }
}
