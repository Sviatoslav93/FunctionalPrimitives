using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public void Should_MatchFailedResult()
    {
        var result = Result.Failed<int>(new Error("test"));

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
