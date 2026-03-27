using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public void Bind_ShouldReturnBoundValue_WhenSome()
    {
        var maybe = Option<int>.Some(3);

        var actual = maybe.Bind(x => Option<int>.Some(x + 2));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(5);
    }

    [Fact]
    public void Bind_ShouldReturnNone_WhenSourceIsNone()
    {
        var maybe = None<int>();
        var invoked = false;

        var actual = maybe.Bind(x =>
        {
            invoked = true;
            return Option<int>.Some(x + 1);
        });

        actual.HasValue.ShouldBeFalse();
        invoked.ShouldBeFalse();
    }

    [Fact]
    public void SelectMany_ShouldFlatten_WhenSome()
    {
        var maybe = Option<int>.Some(2);

        var actual = maybe.SelectMany(x => Option<int>.Some(x * 10));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(20);
    }

    [Fact]
    public void SelectMany_WithProjector_ShouldProject_WhenAllSome()
    {
        var maybe = Option<int>.Some(2);

        var actual = maybe.SelectMany(
            x => Option<int>.Some(x + 3),
            (x, y) => x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public void QueryExpression_ShouldCompileAndProject_WithTwoFromClauses()
    {
        var source = Option<int>.Some(2);

        var actual = from x in source
                     from y in Option<int>.Some(x + 1)
                     select x * y;

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(6);
    }
}
