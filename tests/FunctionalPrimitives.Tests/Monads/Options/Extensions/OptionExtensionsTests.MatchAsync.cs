using FunctionalPrimitives.Monads.Options;
using FunctionalPrimitives.Monads.Options.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options.Extensions;

public partial class OptionExtensionsTests
{
    [Fact]
    public async Task MatchAsync_ShouldInvokeOnSome_WhenSome()
    {
        var task = Task.FromResult(Option<int>.Some(13));

        var actual = await task.MatchAsync(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("V:13");
    }

    [Fact]
    public async Task MatchAsync_ShouldInvokeOnNone_WhenNone()
    {
        var task = Task.FromResult(None<int>());

        var actual = await task.MatchAsync(x => $"V:{x}", () => "NONE");

        actual.ShouldBe("NONE");
    }
}
