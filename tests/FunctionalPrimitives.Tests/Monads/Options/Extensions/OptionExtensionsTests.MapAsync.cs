using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public async Task Map_TaskSource_ShouldProjectValue_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(8));

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
        var task = Task.FromResult(Option<int>.Some(11));

        var actual = await task.Select(x => x * 2);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(22);
    }

    [Fact]
    public async Task QueryExpression_TaskSource_ShouldCompileAndProject_WhenSingleFromClause()
    {
        var source = Task.FromResult(Option<int>.Some(3));

        var actual = await (from x in source
                            select x * 5);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }

    [Fact]
    public async Task MapAsync_WithAsyncProjection_ShouldProjectValue_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(3));

        var actual = await task.MapAsync(async x =>
        {
            await Task.Yield();
            return x * 4;
        });

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(12);
    }

    [Fact]
    public async Task MapAsync_WithAsyncProjection_ShouldReturnNone_WhenNone()
    {
        var task = Task.FromResult(None<int>());

        var actual = await task.MapAsync(async x =>
        {
            await Task.Yield();
            return x + 1;
        });

        actual.HasValue.ShouldBeFalse();
    }

    [Fact]
    public async Task Select_TaskSource_WithAsyncProjection_ShouldProjectValue_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(5));

        var actual = await task.Select(async x =>
        {
            await Task.Yield();
            return x + 10;
        });

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }

    [Fact]
    public async Task MapAsync_WithAsyncProjection_ShouldPropagateException()
    {
        var task = Task.FromResult(Option<int>.Some(1));

        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => task.MapAsync(_ => Task.FromException<int>(new InvalidOperationException("map fail"))));

        ex.Message.ShouldBe("map fail");
    }
}
