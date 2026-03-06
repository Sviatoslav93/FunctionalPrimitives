using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests;

public class MaybeTests
{
    [Fact]
    public void Some_ShouldCreateMaybeWithValue()
    {
        var maybe = Maybe<int>.Some(42);

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
    public void Value_ShouldThrow_WhenHasNoValue()
    {
        var maybe = None<int>();

        var ex = Should.Throw<InvalidOperationException>(() => _ = maybe.Value);
        ex.Message.ShouldBe("No value present.");
    }

    [Fact]
    public void Some_ShouldThrowArgumentNullException_WhenNullPassed()
    {
        Should.Throw<ArgumentNullException>(() => Maybe<string>.Some<string>(null!));
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
        var maybe = Maybe<int>.Some(7);

        var actual = maybe.GetValueOrDefault(10);

        actual.ShouldBe(7);
    }

    [Fact]
    public void FactorySome_ShouldCreateMaybeWithValue()
    {
        var maybe = Maybe.Some(5);

        maybe.HasValue.ShouldBeTrue();
        maybe.Value.ShouldBe(5);
    }

    [Fact]
    public void FactoryNone_ShouldCreateMaybeWithoutValue()
    {
        var maybe = Maybe.None<int>();

        maybe.HasValue.ShouldBeFalse();
    }

    [Fact]
    public void FactorySome_ShouldThrowArgumentNullException_WhenNullPassed()
    {
        Should.Throw<ArgumentNullException>(() => Maybe.Some<string>(null!));
    }
}
