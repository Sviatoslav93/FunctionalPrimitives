using Result.Abstractions;
using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    private const string ErrorMessage = "Input string was not in a correct format.";

    [Fact]
    public void Should_ReturnSuccessResult_When_AllThenPipeSuccess()
    {
        var result = Result<string>.Success("1")
            .Then(x => int.TryParse(x, out var value) ? Result<int>.Success(value) : new Error(ErrorMessage))
            .Then(x => x + 1);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(2);
    }

    [Fact]
    public void Should_ReturnFailedResult_When_AnyOfThenPipeIsFailed()
    {
        var result = Result<string>.Success("1")
            .Then(x => int.TryParse($"{x}i", out var value) ? Result<int>.Success(value) : new Error(ErrorMessage))
            .Then(x => x + 1);

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.Message == ErrorMessage);
    }
}
