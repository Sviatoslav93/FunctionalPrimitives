using FunctionalPrimitives.Monads.Options;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Options;

public class OptionTests
{
    [Fact]
    public void Should_CreateEmpty()
    {
        var maybe = new Option<int>();

        maybe.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Some_ShouldCreateMaybeWithValue()
    {
        var maybe = Option<int>.Some(42);

        maybe.HasValue.ShouldBeTrue();
        maybe.Value.ShouldBe(42);
    }

    [Fact]
    public void None_ShouldCreateMaybeWithoutValue()
    {
        var maybe = None<int>();

        maybe.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Default_ShouldCreateMaybeWithoutValue()
    {
        Option<int> option = default;

        option.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void Value_ShouldThrow_WhenHasNoValue()
    {
        var maybe = None<int>();

        var ex = Should.Throw<InvalidOperationException>(() => _ = maybe.Value);
        ex.Message.ShouldBe("No value present.");
    }

    [Fact]
    public void Some_ShouldThrowArgumentNullException_WhenNullPassed()
    {
        Should.Throw<ArgumentNullException>(() => Option<string>.Some<string>(null!));
    }

    [Fact]
    public void GetValueOrDefault_ShouldReturnProvidedDefault_WhenNone()
    {
        var maybe = None<int>();

        var actual = maybe.GetValueOrDefault(10);

        actual.ShouldBe(10);
    }

    [Fact]
    public void GetValueOrDefault_ShouldReturnValue_WhenSome()
    {
        var maybe = Option<int>.Some(7);

        var actual = maybe.GetValueOrDefault(10);

        actual.ShouldBe(7);
    }

    [Fact]
    public void FactorySome_ShouldCreateMaybeWithValue()
    {
        var maybe = Option.Some(5);

        maybe.HasValue.ShouldBeTrue();
        maybe.Value.ShouldBe(5);
    }

    [Fact]
    public void FactoryNone_ShouldCreateMaybeWithoutValue()
    {
        var maybe = Option.None<int>();

        maybe.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void FactorySome_ShouldThrowArgumentNullException_WhenNullPassed()
    {
        Should.Throw<ArgumentNullException>(() => Option.Some<string>(null!));
    }
}
