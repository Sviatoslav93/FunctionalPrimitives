using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public async Task TapAsync_ShouldExecuteAction_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(6));
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
        var task = Task.FromResult(Option<int>.Some(1));
        var invoked = false;

        await task.TapNoneAsync(() => invoked = true);

        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task TapNoneAsync_WithAsyncAction_ShouldExecuteAction_WhenNone()
    {
        var task = Task.FromResult(None<int>());
        var invoked = false;

        await task.TapNoneAsync(async () =>
        {
            await Task.Yield();
            invoked = true;
        });

        invoked.ShouldBeTrue();
    }

    [Fact]
    public async Task TapNoneAsync_WithAsyncAction_ShouldNotExecuteAction_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(1));
        var invoked = false;

        await task.TapNoneAsync(async () =>
        {
            await Task.Yield();
            invoked = true;
        });

        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task TapNoneAsync_WithAsyncAction_ShouldPropagateException_WhenNone()
    {
        var task = Task.FromResult(None<int>());

        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => task.TapNoneAsync(() => Task.FromException(new InvalidOperationException("boom"))));

        ex.Message.ShouldBe("boom");
    }

    [Fact]
    public async Task Tap_WithAsyncAction_ShouldExecuteAction_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(6));
        var captured = 0;

        var option = await task.Tap(async x =>
        {
            await Task.Yield();
            captured = x;
        });

        captured.ShouldBe(6);
        option.HasValue.ShouldBeTrue();
        option.Value.ShouldBe(6);
    }

    [Fact]
    public async Task Tap_WithAsyncAction_ShouldNotExecuteAction_WhenNone()
    {
        var task = Task.FromResult(None<int>());
        var invoked = false;

        var option = await task.Tap(async _ =>
        {
            await Task.Yield();
            invoked = true;
        });

        invoked.ShouldBeFalse();
        option.HasValue.ShouldBeFalse();
    }

    [Fact]
    public async Task Tap_WithAsyncAction_ShouldPropagateException_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(1));

        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => task.Tap(_ => Task.FromException(new InvalidOperationException("tap fail"))));

        ex.Message.ShouldBe("tap fail");
    }
}
