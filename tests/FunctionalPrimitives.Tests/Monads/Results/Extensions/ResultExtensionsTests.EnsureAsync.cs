using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results.Extensions;
using Shouldly;
using Xunit;

namespace FunctionalPrimitives.Tests.Monads.Results.Extensions;

public partial class ResultExtensionsTests
{
    // EnsureAsync on T (value source) — async predicate + fixed error
    [Fact]
    public async Task EnsureAsync_ValueSource_AsyncPredicate_FixedError_ReturnSuccess_WhenPredicatePasses()
    {
        var result = await 10.EnsureAsync(
            async x =>
            {
                await Task.Delay(0);
                return x > 5;
            },
            new Error("too small"));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task EnsureAsync_ValueSource_AsyncPredicate_FixedError_ReturnFailure_WhenPredicateFails()
    {
        var error = new Error("too small");

        var result = await 2.EnsureAsync(
            async x =>
            {
                await Task.Delay(0);
                return x > 5;
            },
            error);

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldContain(error);
    }

    // EnsureAsync on T (value source) — async predicate + error factory
    [Fact]
    public async Task EnsureAsync_ValueSource_AsyncPredicate_ErrorFactory_ReturnSuccess_WhenPredicatePasses()
    {
        var result = await 10.EnsureAsync(
            async x =>
            {
                await Task.Delay(0);
                return x > 5;
            },
            x => new Error($"value {x} is too small"));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task EnsureAsync_ValueSource_AsyncPredicate_ErrorFactory_ReturnFailure_WhenPredicateFails()
    {
        var result = await 2.EnsureAsync(
            async x =>
            {
                await Task.Delay(0);
                return x > 5;
            },
            x => new Error($"value {x} is too small"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Single().Message.ShouldBe("value 2 is too small");
    }

    [Fact]
    public async Task EnsureAsync_ValueSource_ErrorFactory_InvokesFactoryWithCorrectValue()
    {
        var capturedValue = 0;

        await 7.EnsureAsync(
            async x =>
            {
                await Task.Delay(0);
                return false;
            },
            x =>
            {
                capturedValue = x;
                return new Error("fail");
            });

        capturedValue.ShouldBe(7);
    }

    // EnsureAsync on Task<Result<T>> — sync predicate + fixed error
    [Fact]
    public async Task EnsureAsync_TaskResult_SyncPredicate_FixedError_ReturnSuccess_WhenPredicatePasses()
    {
        var task = Task.FromResult(Success(10));

        var result = await task.EnsureAsync(x => x > 5, new Error("too small"));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_SyncPredicate_FixedError_ReturnFailure_WhenPredicateFails()
    {
        var error = new Error("too small");
        var task = Task.FromResult(Success(2));

        var result = await task.EnsureAsync(x => x > 5, error);

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldContain(error);
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_SyncPredicate_FixedError_PropagatesExistingFailure()
    {
        var originalError = new Error("original");
        var task = Task.FromResult(Failure<int>(originalError));
        var invoked = false;

        var result = await task.EnsureAsync(
            x =>
            {
                invoked = true;
                return x > 0;
            },
            new Error("predicate failed"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.ShouldContain(originalError);
        invoked.ShouldBeFalse();
    }

    // EnsureAsync on Task<Result<T>> — sync predicate + error factory
    [Fact]
    public async Task EnsureAsync_TaskResult_SyncPredicate_ErrorFactory_ReturnSuccess_WhenPredicatePasses()
    {
        var task = Task.FromResult(Success(10));

        var result = await task.EnsureAsync(x => x > 5, x => new Error($"{x} is too small"));

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_SyncPredicate_ErrorFactory_ReturnFailure_WhenPredicateFails()
    {
        var task = Task.FromResult(Success(2));

        var result = await task.EnsureAsync(x => x > 5, x => new Error($"{x} is too small"));

        result.IsSuccess.ShouldBeFalse();
        result.Errors.Single().Message.ShouldBe("2 is too small");
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_ErrorFactory_InvokesFactoryWithCorrectValue()
    {
        var capturedValue = 0;
        var task = Task.FromResult(Success(9));

        await task.EnsureAsync(
            x => false,
            x =>
            {
                capturedValue = x;
                return new Error("fail");
            });

        capturedValue.ShouldBe(9);
    }
}
