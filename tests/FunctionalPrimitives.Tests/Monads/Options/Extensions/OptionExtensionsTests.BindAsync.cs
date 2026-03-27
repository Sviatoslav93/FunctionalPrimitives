using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public async Task Bind_TaskSource_ShouldReturnBoundValue_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(2));

        var actual = await task.BindAsync(x => Option<int>.Some(x + 7));

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
            return Option<int>.Some(x + 1);
        });

        actual.HasValue.ShouldBeFalse();
        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task SelectMany_TaskSource_ShouldFlatten_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(4));

        var actual = await task.SelectMany(x => Option<int>.Some(x * 2));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(8);
    }

    [Fact]
    public async Task SelectMany_TaskSource_WithProjector_ShouldProject_WhenAllSome()
    {
        var task = Task.FromResult(Option<int>.Some(3));

        var actual = await task.SelectMany(
            x => Option<int>.Some(x + 2),
            (x, y) => x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(15);
    }

    [Fact]
    public async Task QueryExpression_TaskSource_ShouldCompileAndProject_WithTwoFromClauses()
    {
        var source = Task.FromResult(Option<int>.Some(2));

        var actual = await (from x in source
                            from y in Option<int>.Some(x + 3)
                            select x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(10);
    }

    [Fact]
    public async Task BindAsync_WithAsyncBinder_ShouldReturnBoundValue_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(2));

        var actual = await task.BindAsync(x => Task.FromResult(Option<int>.Some(x + 5)));

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(7);
    }

    [Fact]
    public async Task BindAsync_WithAsyncBinder_ShouldNotInvokeBinder_WhenSourceIsNone()
    {
        var task = Task.FromResult(None<int>());
        var invoked = false;

        var actual = await task.BindAsync(x =>
        {
            invoked = true;
            return Task.FromResult(Option<int>.Some(x + 1));
        });

        actual.HasValue.ShouldBeFalse();
        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task SelectMany_TaskSource_WithAsyncBinderAndProjector_ShouldProject_WhenAllSome()
    {
        var task = Task.FromResult(Option<int>.Some(4));

        var actual = await task.SelectMany(
            x => Task.FromResult(Option<int>.Some(x + 1)),
            (x, y) => x * y);

        actual.HasValue.ShouldBeTrue();
        actual.Value.ShouldBe(20);
    }

    [Fact]
    public async Task BindAsync_WithAsyncBinder_ShouldPropagateException()
    {
        var task = Task.FromResult(Option<int>.Some(2));

        var ex = await Should.ThrowAsync<InvalidOperationException>(
            () => task.BindAsync(_ => Task.FromException<Option<int>>(new InvalidOperationException("bind fail"))));

        ex.Message.ShouldBe("bind fail");
    }
}
