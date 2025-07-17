using Xunit;

namespace Result.Tests;

public class ErrorTests
{
    [Fact]
    public void Should_CreateErrorWithMessage()
    {
        const string message = "Error message";

        var error = new Error(message);

        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void Should_ReturnMessageWhenToStringIsCalled()
    {
        const string message = "Error message";
        var error = new Error(message);

        var result = error.ToString();

        Assert.Equal(message, result);
    }
}
