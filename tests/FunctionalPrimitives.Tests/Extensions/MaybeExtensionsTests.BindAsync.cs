using FunctionalPrimitives.Extensions.Maybe;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class MaybeExtensionsTests
{
    [Fact]
    public async Task Bind_TaskSource_ShouldReturnBoundValue_WhenSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(2));

        var actual = await task.BindAsync(x => Maybe<int>.Some(x + 7));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(9);
    }

    [Fact]
    public async Task Bind_TaskSource_ShouldReturnNone_WhenSourceIsNone()
    {
        var task = Task.FromResult(None<int>());
        var invoked = false;

        var actual = await task.BindAsync(x =>
        {
            invoked = true;
            return Maybe<int>.Some(x + 1);
        });

        actual.HasValue.ShouldBeFalse();
        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task SelectMany_TaskSource_ShouldFlatten_WhenSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(4));

        var actual = await task.SelectMany(x => Maybe<int>.Some(x * 2));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(8);
    }

    [Fact]
    public async Task SelectMany_TaskSource_WithProjector_ShouldProject_WhenAllSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(3));

        var actual = await task.SelectMany(
            x => Maybe<int>.Some(x + 2),
            (x, y) => x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }

    [Fact]
    public async Task QueryExpression_TaskSource_ShouldCompileAndProject_WithTwoFromClauses()
    {
        var source = Task.FromResult(Maybe<int>.Some(2));

        var actual = await (from x in source
                            from y in Maybe<int>.Some(x + 3)
                            select x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }
}
