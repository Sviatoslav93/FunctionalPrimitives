using FunctionalPrimitives.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class MaybeExtensionsTests
{
    [Fact]
    public void Tap_ShouldExecuteAction_WhenSome()
    {
        var maybe = Maybe<int>.Some(9);
        var captured = 0;

        maybe.Tap(x => captured = x);

        captured.ShouldBe(9);
    }

    [Fact]
    public void Tap_ShouldNotExecuteAction_WhenNone()
    {
        var maybe = Maybe<int>.None;
        var invoked = false;

        maybe.Tap(_ => invoked = true);

        invoked.ShouldBeFalse();
    }

    [Fact]
    public void TapNone_ShouldExecuteAction_WhenNone()
    {
        var maybe = Maybe<int>.None;
        var invoked = false;

        maybe.TapNone(() => invoked = true);

        invoked.ShouldBeTrue();
    }

    [Fact]
    public void TapNone_ShouldNotExecuteAction_WhenSome()
    {
        var maybe = Maybe<int>.Some(1);
        var invoked = false;

        maybe.TapNone(() => invoked = true);

        invoked.ShouldBeFalse();
    }
}
