using FunctionalPrimitives.Extensions.Result;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Should_MatchFailedResult()
    {
        var result = Result.Failure<int>(new Error("test"));

        var value = result.Match(
            x => x,
            _ => 0);

        value.ShouldBe(0);
    }

    [Fact]
    public void Should_MatchSuccessResult()
    {
        var result = Result.Success(1);

        var value = result.Match(
            x => x,
            _ => 0);

        value.ShouldBe(1);
    }
}
