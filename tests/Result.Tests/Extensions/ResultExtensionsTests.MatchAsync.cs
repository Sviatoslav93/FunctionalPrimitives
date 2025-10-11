using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public async Task Should_MatchSuccessAsync_From_Result()
    {
        var result = Result.Success("1");

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(1);

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => 0))
            .ShouldBe(1);

        (await result.MatchAsync(
                onSuccess: int.Parse,
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(1);
    }

    [Fact]
    public async Task Should_MatchFailedAsync_From_Result()
    {
        var result = Result.Failure<string>(new Error("test"));

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(0);

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => 0))
            .ShouldBe(0);

        (await result.MatchAsync(
                onSuccess: int.Parse,
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(0);
    }

    [Fact]
    public async Task Should_MatchSuccessAsync_From_ResultTask()
    {
        var result = Task.FromResult(Result.Success("1"));

        (await result.MatchAsync(
                onSuccess: int.Parse,
                onFailure: _ => 0))
            .ShouldBe(1);

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(1);

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => 0))
            .ShouldBe(1);

        (await result.MatchAsync(
                onSuccess: int.Parse,
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(1);
    }

    [Fact]
    public async Task Should_MatchFailedAsync_From_ResultTask()
    {
        var result = Task.FromResult(Result.Failure<string>(new Error("test")));

        (await result.MatchAsync(
                onSuccess: int.Parse,
                onFailure: _ => 0))
            .ShouldBe(0);

        (await result.MatchAsync(
                onSuccess: v => Task.FromResult(int.Parse(v)),
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(0);

        (await result.MatchAsync(
                onSuccess: x => Task.FromResult(int.Parse(x)),
                onFailure: _ => 0))
            .ShouldBe(0);

        (await result.MatchAsync(
                onSuccess: int.Parse,
                onFailure: _ => Task.FromResult(0)))
            .ShouldBe(0);
    }
}
