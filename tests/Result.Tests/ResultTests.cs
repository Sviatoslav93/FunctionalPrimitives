using Shouldly;
using Xunit;

namespace Result.Tests;

public partial class ResultTests
{
    [Fact]
    public void Should_CreateSuccessResult()
    {
        var result = Result.Success();

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_CreateFailedResult()
    {
        var result1 = Result.Failure(new Error("test"));
        var result2 = Result.Failure(new List<Error> { new("test") });
        var result3 = Result.Failure(new Error("test"), new Error("test2"));

        result1.IsSuccess.ShouldBeFalse();
        result2.IsSuccess.ShouldBeFalse();
        result3.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public void Should_ReturnSuccesResult_When_TryActionIsSuccess()
    {
        static void Act()
        {
            // do nothing =)
        }

        var res = Result.Try(Act, ex => new Error(ex.Message));

        res.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Should_ReturnFailedResult_When_TryActionIsFailed()
    {
        var msg = "exceprion";

        void Act()
        {
            throw new Exception(msg);
        }

        var res = Result.Try(Act, ex => new Error(ex.Message));

        res.IsSuccess.ShouldBeTrue();
        res.Errors[0].Message.ShouldBe<string>(msg);
    }
}
