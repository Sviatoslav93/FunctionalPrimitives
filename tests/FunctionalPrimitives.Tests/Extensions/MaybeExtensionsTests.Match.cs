using FunctionalPrimitives.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class MaybeExtensionsTests
{
    [Fact]
    public void Match_ShouldInvokeOnSome_WhenSome()
    {
        var maybe = Maybe<int>.Some(7);

        var actual = maybe.Match(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("V:7");
    }

    [Fact]
    public void Match_ShouldInvokeOnNone_WhenNone()
    {
        var maybe = Maybe<int>.None;

        var actual = maybe.Match(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("NONE");
    }
}
