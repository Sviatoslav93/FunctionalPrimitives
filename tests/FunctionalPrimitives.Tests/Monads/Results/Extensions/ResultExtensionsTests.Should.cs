using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;
using FunctionalPrimitives.Monads.Results.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Results.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Should_ValueSource_ShouldReturnSuccess_WhenPredicateIsTrue()
    {
        var actual = 10.Ensure(x => x > 5, new Error("predicate failed"));

        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public void Should_ValueSource_ShouldReturnFailure_WhenPredicateIsFalse()
    {
        var error = new Error("predicate failed");

        var actual = 2.Ensure(x => x > 5, error);

        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public void Should_ResultSource_ShouldReturnSuccess_WhenPredicateIsTrue()
    {
        var result = Result.Success(10);

        var actual = result.Ensure(x => x % 2 == 0, new Error("predicate failed"));

        actual.IsSuccess.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public void Should_ResultSource_ShouldReturnFailure_WhenPredicateIsFalse()
    {
        var error = new Error("predicate failed");
        var result = Result.Success(3);

        var actual = result.Ensure(x => x % 2 == 0, error);

        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldContain(error);
    }

    [Fact]
    public void Should_ResultSource_ShouldPropagateOriginalFailure_WithoutInvokingPredicate()
    {
        var originalError = new Error("original failure");
        var result = Result.Failure<int>(originalError);
        var invoked = false;

        var actual = result.Ensure(
            x =>
            {
                invoked = true;
                return x > 0;
            },
            new Error("predicate failed"));

        actual.IsSuccess.ShouldBeFalse();
        actual.Errors.ShouldContain(originalError);
        invoked.ShouldBeFalse();
    }
}
