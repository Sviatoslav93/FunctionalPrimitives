using FunctionalPrimitives;
using Xunit;

namespace FunctionalPrimitives.Tests;

public class NoneTests
{
    [Fact]
    public void Value_ReturnsNoneInstance()
    {
        var none = None.Value;

        Assert.IsType<None>(none);
    }

    [Fact]
    public async Task Task_ReturnsCompletedTask()
    {
        var task = None.Task;

        Assert.True(task.IsCompleted);
        var none = await task;
        Assert.IsType<None>(none);
    }

    [Fact]
    public void EqualityOperator_AlwaysReturnsTrue()
    {
        var first = None.Value;
        var second = None.Value;
        var third = default(None);

        Assert.True(first == second);
        Assert.True(first == third);
        Assert.True(second == third);
    }

    [Fact]
    public void InequalityOperator_AlwaysReturnsFalse()
    {
        var first = None.Value;
        var second = None.Value;
        var third = default(None);

        Assert.False(first != second);
        Assert.False(first != third);
        Assert.False(second != third);
    }

    [Fact]
    public void Equals_WithNone_ReturnsTrue()
    {
        var first = None.Value;
        var second = None.Value;

        Assert.True(first.Equals(second));
    }

    [Fact]
    public void Equals_WithObject_ReturnsTrue_WhenObjectIsNone()
    {
        var none = None.Value;
        object obj = default(None);

        Assert.True(none.Equals(obj));
    }

    [Fact]
    public void Equals_WithObject_ReturnsFalse_WhenObjectIsNotNone()
    {
        var none = None.Value;
        object obj = "not a none";

        Assert.False(none.Equals(obj));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var none = None.Value;

        Assert.False(none.Equals(null));
    }

    [Fact]
    public void GetHashCode_AlwaysReturnsZero()
    {
        var first = None.Value;
        var second = default(None);

        Assert.Equal(0, first.GetHashCode());
        Assert.Equal(0, second.GetHashCode());
    }

    [Fact]
    public void CompareTo_AlwaysReturnsZero()
    {
        var first = None.Value;
        var second = None.Value;

        Assert.Equal(0, first.CompareTo(second));
    }

    [Fact]
    public void CompareTo_WithObject_AlwaysReturnsZero()
    {
        var none = None.Value;
        IComparable comparable = none;

        Assert.Equal(0, comparable.CompareTo(None.Value));
        Assert.Equal(0, comparable.CompareTo(default(None)));
    }

    [Fact]
    public void ToString_ReturnsUnitNotation()
    {
        var none = None.Value;

        Assert.Equal("()", none.ToString());
    }

    [Fact]
    public void Default_IsEqualToValue()
    {
        var defaultNone = default(None);
        var valueNone = None.Value;

        Assert.True(defaultNone == valueNone);
        Assert.True(defaultNone.Equals(valueNone));
    }
}
