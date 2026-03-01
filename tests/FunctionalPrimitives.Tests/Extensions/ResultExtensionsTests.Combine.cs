using FunctionalPrimitives.ResultExtensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Should_Combine_ResultsArray_WithoutFailures()
    {
        var results = new[] { Result.Success(1), Result.Success(2), Result.Success(3) };
        var combined = results.Combine();

        combined.IsSuccess.ShouldBeTrue();
        combined.Value.ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void Should_Combine_ResultsArray_WithFailures()
    {
        var results = new[]
        {
            Result.Success(1),
            Result.Failure<int>(new Error("Error")),
            Result.Success(3),
            Result.Success(4),
            Result.Failure<int>(new Error("Error 2")),
        };
        var combined = results.Combine();

        combined.IsSuccess.ShouldBeFalse();
        combined.Errors.Count().ShouldBe(2);
    }

    [Fact]
    public void Should_Combine_Results_WithFailures()
    {
        var result1 = Result.Success(1);
        var result2 = Result.Success(2);

        var result3 = result1.Combine(result2);

        result3.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_Combine_Results_WithoutFailures()
    {
        var result1 = Result.Success(1);
        var result2 = Result.Failure<int>(new Error("error"));

        var result3 = result1.Combine(result2);

        result3.IsSuccess.ShouldBeFalse();
    }
}
