using FunctionalPrimitives.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class MaybeExtensionsTests
{
    [Fact]
    public void Bind_ShouldReturnBoundValue_WhenSome()
    {
        var maybe = Maybe<int>.Some(3);

        var actual = maybe.Bind(x => Maybe<int>.Some(x + 2));

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
            return Maybe<int>.Some(x + 1);
        });

        actual.HasValue.ShouldBeFalse();
        invoked.ShouldBeFalse();
    }

    [Fact]
    public void SelectMany_ShouldFlatten_WhenSome()
    {
        var maybe = Maybe<int>.Some(2);

        var actual = maybe.SelectMany(x => Maybe<int>.Some(x * 10));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(20);
    }

    [Fact]
    public void SelectMany_WithProjector_ShouldProject_WhenAllSome()
    {
        var maybe = Maybe<int>.Some(2);

        var actual = maybe.SelectMany(
            x => Maybe<int>.Some(x + 3),
            (x, y) => x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public void QueryExpression_ShouldCompileAndProject_WithTwoFromClauses()
    {
        var source = Maybe<int>.Some(2);

        var actual = from x in source
                     from y in Maybe<int>.Some(x + 1)
                     select x * y;

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(6);
    }
}
