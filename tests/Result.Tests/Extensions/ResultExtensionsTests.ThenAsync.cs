using Result.Extensions;
using Shouldly;
using Xunit;

namespace Result.Tests.Extensions;

public partial class ResultExtensionsTests
{
    [Fact]
    public async Task ThenAsync_ShouldReturnSuccessResult_When_OnSuccessIsSuccessWithTaskNextValue()
    {
        var result1 = await Result.Success("1")
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(value)
                : Task.FromResult(0));

        var result2 = await Task.FromResult(Result.Success("1"))
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(value)
                : Task.FromResult(0));

        result1.IsSuccess.ShouldBeTrue();
        result1.Value.ShouldBe(1);

        result2.IsSuccess.ShouldBeTrue();
        result2.Value.ShouldBe(1);
    }

    [Fact]
    public async Task ThenAsync_ShouldReturnSuccessResult_When_OnSuccessIsSuccessWithTaskResultNextValue()
    {
        var resultSuccess1 = await Result.Success("1")
            .ThenAsync(static x => int.TryParse(x, out var value)
                ? Task.FromResult(Result.Success(value))
                : Task.FromResult(Result.Failure<int>(new Error(ErrorMessage))));

        var resultSuccess2 = await Task.FromResult(Result.Success("1"))
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(Result.Success(value))
                : Task.FromResult(Result.Failure<int>(new Error(ErrorMessage))));

        var resultFailed1 = await Result.Success("1i")
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(Result.Success(value))
                : Task.FromResult(Result.Failure<int>(new Error(ErrorMessage))));

        var resultFailed2 = await Task.FromResult(Result.Success("1i"))
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(Result.Success(value))
                : Task.FromResult(Result.Failure<int>(new Error(ErrorMessage))));

        resultSuccess1.IsSuccess.ShouldBeTrue();
        resultSuccess1.Value.ShouldBe(1);

        resultFailed1.IsSuccess.ShouldBeFalse();
        resultFailed1.Errors.ShouldHaveSingleItem();

        resultSuccess2.IsSuccess.ShouldBeTrue();
        resultSuccess2.Value.ShouldBe(1);

        resultFailed2.IsSuccess.ShouldBeFalse();
        resultFailed2.Errors.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task ThenAsync_ShouldReturnFailedResult_When_OnSuccessIsFailedWithTaskResultNextValue()
    {
        var resultFailed = await Result.Success("1i")
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(Result.Success(value))
                : Task.FromResult(Result.Failure<int>(new Error(ErrorMessage))));

        resultFailed.IsSuccess.ShouldBeFalse();
        resultFailed.Errors.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task ThenAsync_ShouldReturnSuccessResult_When_OnSuccessIsSuccessWithNexValue()
    {
        var resultSuccess = await Task.FromResult(Result.Success("1"))
            .ThenAsync(x => int.TryParse(x, out var value)
                ? value
                : 0);

        resultSuccess.IsSuccess.ShouldBeTrue();
        resultSuccess.Value.ShouldBe(1);
    }

    [Fact]
    public async Task ThenAsync_ShouldReturnFailedResult_WhenAtLeastOneChainFailed()
    {
        var result = await Result.Success("1i")
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(value)
                : Task.FromResult(0))
            .ThenAsync(x => x != 0
                ? Result.Success(10 / x)
                : Result.Failure<int>(new Error("Division by zero")))
            .ThenAsync(x => x.ToString());

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task ThenAsync_ShouldReturnSuccessResult_WhenAllChainsAreSuccess()
    {
        var result = await Result.Success("1")
            .ThenAsync(x => int.TryParse(x, out var value)
                ? Task.FromResult(value)
                : Task.FromResult(0))
            .ThenAsync(x => x != 0
                ? Result.Success(10 / x)
                : Result.Failure<int>(new Error("Division by zero")))
            .ThenAsync(x => x.ToString());

        result.IsSuccess.ShouldBeTrue();
        result.Errors.Count().ShouldBe(0);
    }
}
