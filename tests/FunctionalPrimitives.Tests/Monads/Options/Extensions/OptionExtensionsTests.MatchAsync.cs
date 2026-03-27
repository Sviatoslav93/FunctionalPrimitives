using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public async Task MatchAsync_ShouldInvokeOnSome_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(13));

        var actual = await task.MatchAsync(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("V:13");
    }

    [Fact]
    public async Task MatchAsync_ShouldInvokeOnNone_WhenNone()
    {
        var task = Task.FromResult(None<int>());

        var actual = await task.MatchAsync(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("NONE");
    }

    [Fact]
    public async Task MatchAsync_WithAsyncOnSomeAndSyncOnNone_ShouldInvokeOnSome_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(4));

        var actual = await task.MatchAsync(
            async x =>
            {
                await Task.Yield();
                return $"S:{x}";
            },
            () => "NONE");

        actual.ShouldBe("S:4");
    }

    [Fact]
    public async Task MatchAsync_WithSyncOnSomeAndAsyncOnNone_ShouldInvokeOnNone_WhenNone()
    {
        var task = Task.FromResult(None<int>());

        var actual = await task.MatchAsync(
            x => $"S:{x}",
            async () =>
            {
                await Task.Yield();
                return "NONE";
            });

        actual.ShouldBe("NONE");
    }

    [Fact]
    public async Task MatchAsync_WithBothAsyncHandlers_ShouldUseCorrectPath_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(9));

        var actual = await task.MatchAsync(
            async x =>
            {
                await Task.Yield();
                return $"V:{x}";
            },
            async () =>
            {
                await Task.Yield();
                return "NONE";
            });

        actual.ShouldBe("V:9");
    }

    [Fact]
    public async Task MatchAsync_ShouldPropagateException_FromAsyncOnSome()
    {
        var task = Task.FromResult(Option<int>.Some(2));

        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => task.MatchAsync(
                _ => Task.FromException<string>(new InvalidOperationException("some fail")),
                () => "NONE"));

        ex.Message.ShouldBe("some fail");
    }

    [Fact]
    public async Task MatchAsync_ShouldPropagateException_FromAsyncOnNone()
    {
        var task = Task.FromResult(None<int>());

        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => task.MatchAsync(
                x => $"V:{x}",
                () => Task.FromException<string>(new InvalidOperationException("none fail"))));

        ex.Message.ShouldBe("none fail");
    }
}
