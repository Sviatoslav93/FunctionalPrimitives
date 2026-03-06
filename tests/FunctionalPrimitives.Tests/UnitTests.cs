using FunctionalPrimitives;
using Xunit;

namespace FunctionalPrimitives.Tests;

public class UnitTests
{
    [Fact]
    public void Value_ReturnsNoneInstance()
    {
        var none = Unit.Value;

        Assert.IsType<Unit>(none);
    }

    [Fact]
    public async Task Task_ReturnsCompletedTask()
    {
        var task = Unit.Task;

        Assert.True(task.IsCompleted);
        var none = await task;
        Assert.IsType<Unit>(none);
    }

    [Fact]
    public void EqualityOperator_AlwaysReturnsTrue()
    {
        var first = Unit.Value;
        var second = Unit.Value;
        var third = default(Unit);

        Assert.True(first == second);
        Assert.True(first == third);
        Assert.True(second == third);
    }

    [Fact]
    public void InequalityOperator_AlwaysReturnsFalse()
    {
        var first = Unit.Value;
        var second = Unit.Value;
        var third = default(Unit);

        Assert.False(first != second);
        Assert.False(first != third);
        Assert.False(second != third);
    }

    [Fact]
    public void Equals_WithNone_ReturnsTrue()
    {
        var first = Unit.Value;
        var second = Unit.Value;

        Assert.True(first.Equals(second));
    }

    [Fact]
    public void Equals_WithObject_ReturnsTrue_WhenObjectIsNone()
    {
        var none = Unit.Value;
        object obj = default(Unit);

        Assert.True(none.Equals(obj));
    }

    [Fact]
    public void Equals_WithObject_ReturnsFalse_WhenObjectIsNotNone()
    {
        var none = Unit.Value;
        object obj = "not a none";

        Assert.False(none.Equals(obj));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var none = Unit.Value;

        Assert.False(none.Equals(null));
    }

    [Fact]
    public void GetHashCode_AlwaysReturnsZero()
    {
        var first = Unit.Value;
        var second = default(Unit);

        Assert.Equal(0, first.GetHashCode());
        Assert.Equal(0, second.GetHashCode());
    }

    [Fact]
    public void CompareTo_AlwaysReturnsZero()
    {
        var first = Unit.Value;
        var second = Unit.Value;

        Assert.Equal(0, first.CompareTo(second));
    }

    [Fact]
    public void CompareTo_WithObject_AlwaysReturnsZero()
    {
        var none = Unit.Value;
        IComparable comparable = none;

        Assert.Equal(0, comparable.CompareTo(Unit.Value));
        Assert.Equal(0, comparable.CompareTo(default(Unit)));
    }

    [Fact]
    public void ToString_ReturnsUnitNotation()
    {
        var none = Unit.Value;

        Assert.Equal("()", none.ToString());
    }

    [Fact]
    public void Default_IsEqualToValue()
    {
        var defaultNone = default(Unit);
        var valueNone = Unit.Value;

        Assert.True(defaultNone == valueNone);
        Assert.True(defaultNone.Equals(valueNone));
    }
}
