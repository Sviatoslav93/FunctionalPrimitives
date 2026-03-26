using FunctionalPrimitives.Errors;
using Shouldly;
using Xunit;
using Exception = System.Exception;

namespace FunctionalPrimitives.Tests.Errors;

public class ErrorTests
{
    [Fact]
    public void Should_CreateError()
    {
        const string message = "Error message";

        var error = new Error(message);

        error.Message.ShouldBe(message);
        error.Type.ShouldBe("default");
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

    [Fact]
    public void Should_ConvertStringToError()
    {
        const string message = "Error message";

        Error error = message;

        error.Message.ShouldBe(message);
        error.Code.ShouldBe(string.Empty);
    }

    [Fact]
    public void Should_ConvertExceptionToError()
    {
        var ex = new Exception("Some exception");
        UnexpectedError error = ex;

        error.Message.ShouldBe(ex.Message);
        error.Code.ShouldBe(ex.GetType().Name);
        error.Type.ShouldBe("unexpected");
    }
}
