using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public void Match_ShouldInvokeOnSome_WhenSome()
    {
        var maybe = Option<int>.Some(7);

        var actual = maybe.Match(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("V:7");
    }

    [Fact]
    public void Match_ShouldInvokeOnNone_WhenNone()
    {
        var maybe = None<int>();

        var actual = maybe.Match(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("NONE");
    }
}
