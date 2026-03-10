using FunctionalPrimitives.Extensions.Maybe;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class MaybeExtensionsTests
{
    [Fact]
    public async Task TapAsync_ShouldExecuteAction_WhenSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(6));
        var captured = 0;

        await task.TapAsync(x => captured = x);

        captured.ShouldBe(6);
    }

    [Fact]
    public async Task TapAsync_ShouldNotExecuteAction_WhenNone()
    {
        var task = Task.FromResult(None<int>());
        var invoked = false;

        await task.TapAsync(_ => invoked = true);

        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task TapNoneAsync_ShouldExecuteAction_WhenNone()
    {
        var task = Task.FromResult(None<int>());
        var invoked = false;

        await task.TapNoneAsync(() => invoked = true);

        invoked.ShouldBeTrue();
    }

    [Fact]
    public async Task TapNoneAsync_ShouldNotExecuteAction_WhenSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(1));
        var invoked = false;

        await task.TapNoneAsync(() => invoked = true);

        invoked.ShouldBeFalse();
    }
}
