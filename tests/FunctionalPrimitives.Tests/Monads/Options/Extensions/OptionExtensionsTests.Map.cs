using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public void Map_ShouldProjectValue_WhenSome()
    {
        var maybe = Option<int>.Some(10);

        var actual = maybe.Map(x => x * 2);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(20);
    }

    [Fact]
    public void Map_ShouldReturnNone_WhenNone()
    {
        var maybe = None<int>();

        var actual = maybe.Map(x => x * 2);

        actual.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Select_ShouldProjectValue_WhenSome()
    {
        var maybe = Option<int>.Some(5);

        var actual = maybe.Select(x => x + 1);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(6);
    }

    [Fact]
    public void QueryExpression_ShouldCompileAndProject_WhenSingleFromClause()
    {
        var source = Option<int>.Some(4);

        var actual = from x in source
                     select x * 3;

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(12);
    }
}
