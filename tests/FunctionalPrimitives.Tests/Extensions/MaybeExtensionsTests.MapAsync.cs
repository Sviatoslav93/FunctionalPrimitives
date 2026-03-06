using FunctionalPrimitives.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Extensions;

public partial class MaybeExtensionsTests
{
    [Fact]
    public async Task Map_TaskSource_ShouldProjectValue_WhenSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(8));

        var actual = await task.MapAsync(x => x + 2);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public async Task Map_TaskSource_ShouldReturnNone_WhenNone()
    {
        var task = Task.FromResult(None<int>());

        var actual = await task.MapAsync(x => x + 2);

        actual.HasValue.ShouldBeFalse();
    }

    [Fact]
    public async Task Select_TaskSource_ShouldProjectValue_WhenSome()
    {
        var task = Task.FromResult(Maybe<int>.Some(11));

        var actual = await task.Select(x => x * 2);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(22);
    }

    [Fact]
    public async Task QueryExpression_TaskSource_ShouldCompileAndProject_WhenSingleFromClause()
    {
        var source = Task.FromResult(Maybe<int>.Some(3));

        var actual = await (from x in source
                            select x * 5);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }
}
