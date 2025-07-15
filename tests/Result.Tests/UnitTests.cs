using Shouldly;
using Xunit;

namespace Result.Tests;

public class UnitTests
{
    [Fact]
    public void TwoNothingsValuesShouldBeEqual()
    {
        var first = Unit.Value;
        var second = Unit.Value;

        (first == second).ShouldBeTrue();
        (first != second).ShouldBeFalse();
        first.Equals(second).ShouldBeTrue();
        first.CompareTo(second).ShouldBe(0);
        first.Equals(new object()).ShouldBeFalse();
    }

    [Fact]
    public void ShouldBeImplementedIComparable()
    {
        var nothing = Unit.Value;
        var toComparable = (IComparable)nothing;
        toComparable.CompareTo(new object()).ShouldBe(0);
    }

    [Fact]
    public async Task ShouldReturnTaskNothing()
    {
        var task = Unit.Task;
        var nothing = await task;
        nothing.ShouldBe(Unit.Value);
    }

    [Fact]
    public void ShouldBeOverridenToString()
    {
        var nothing = Unit.Value;
        nothing.ToString().ShouldBe("()");
    }
}
