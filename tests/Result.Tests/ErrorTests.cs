using Shouldly;
using Xunit;

namespace Result.Tests;

public class ErrorTests
{
    [Fact]
    public void Should_CreateError()
    {
        const string message = "Error message";

        var error = new Error(message);

        error.MemberName.ShouldBe(nameof(Should_CreateError));
        error.Message.ShouldBe(message);
    }

    [Fact]
    public void Should_CreateErrorWithCode()
    {
        const string message = "Error message";
        const string code = "code";

        var error = new Error(message, code);

        error.Message.ShouldBe(message);
        error.Code.ShouldBe(code);
    }
}
