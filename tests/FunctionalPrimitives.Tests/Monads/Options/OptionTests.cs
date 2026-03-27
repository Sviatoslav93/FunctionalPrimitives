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

    // Equality
    [Fact]
    public void Equals_ShouldBeTrue_WhenBothSomeWithSameValue()
    {
        var a = Option.Some(42);
        var b = Option.Some(42);

        a.Equals(b).ShouldBeTrue();
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenValuesAreDifferent()
    {
        var a = Option.Some(42);
        var b = Option.Some(99);

        a.Equals(b).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenOneIsNone()
    {
        var some = Option.Some(42);
        var none = Option.None<int>();

        some.Equals(none).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenBothNone()
    {
        var a = Option.None<int>();
        var b = Option.None<int>();

        a.Equals(b).ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_ShouldBeTrue_WhenBoxedOptionWithSameValue()
    {
        var a = Option.Some(42);
        object b = Option.Some(42);

        a.Equals(b).ShouldBeTrue();
    }

    // GetHashCode
    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenBothSomeWithSameValue()
    {
        var a = Option.Some(42);
        var b = Option.Some(42);

        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ShouldDiffer_WhenSomeAndNone()
    {
        var some = Option.Some(42);
        var none = Option.None<int>();

        some.GetHashCode().ShouldNotBe(none.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ShouldAllowUseInHashSet()
    {
        var set = new HashSet<Option<int>>
        {
            Option.Some(1),
            Option.Some(1),
            Option.Some(2),
            Option.None<int>(),
            Option.None<int>(),
        };

        set.Count.ShouldBe(3);
    }

    // Operators
    [Fact]
    public void EqualityOperator_ShouldReturnTrue_WhenSomeWithSameValue()
    {
        var a = Option.Some(42);
        var b = Option.Some(42);

        (a == b).ShouldBeTrue();
    }

    [Fact]
    public void EqualityOperator_ShouldReturnFalse_WhenDifferentValues()
    {
        var a = Option.Some(1);
        var b = Option.Some(2);

        (a == b).ShouldBeFalse();
    }

    [Fact]
    public void InequalityOperator_ShouldReturnTrue_WhenSomeAndNone()
    {
        var some = Option.Some(42);
        var none = Option.None<int>();

        (some != none).ShouldBeTrue();
    }

    [Fact]
    public void InequalityOperator_ShouldReturnFalse_WhenBothNone()
    {
        var a = Option.None<int>();
        var b = Option.None<int>();

        (a != b).ShouldBeFalse();
    }

    // ToString
    [Fact]
    public void ToString_ShouldReturnSomeWithValue_WhenHasValue()
    {
        var option = Option.Some(42);

        option.ToString().ShouldBe("Some(42)");
    }

    [Fact]
    public void ToString_ShouldReturnNone_WhenNoValue()
    {
        var option = Option.None<int>();

        option.ToString().ShouldBe("None");
    }

    // Deconstruct
    [Fact]
    public void Deconstruct_ShouldProvideHasValueTrueAndValue_WhenSome()
    {
        var option = Option.Some(42);

        var (hasValue, value) = option;

        hasValue.ShouldBeTrue();
        value.ShouldBe(42);
    }

    [Fact]
    public void Deconstruct_ShouldProvideHasValueFalseAndDefault_WhenNone()
    {
        var option = Option.None<int>();

        var (hasValue, value) = option;

        hasValue.ShouldBeFalse();
        value.ShouldBe(default);
    }

    // Implicit operator
    [Fact]
    public void ImplicitOperator_ShouldCreateSome_WhenValueProvided()
    {
        Option<int> option = 42;

        option.HasValue.ShouldBeTrue();
        option.Value.ShouldBe(42);
    }

    [Fact]
    public void ImplicitOperator_ShouldThrow_WhenNullProvided()
    {
        Should.Throw<ArgumentNullException>(() => { Option<string> option = (string)null!; });
    }
}
