using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result<TValue>> RecoverAsync<TValue>(
        this Result<TValue> result,
        Func<IEnumerable<Error>, Task<TValue>> recover)
    {
        if (result.IsSuccess)
            return result;

        var recoveredValue = await recover(result.Errors).ConfigureAwait(false);
        return Result.Success(recoveredValue);
    }

    public static Task<Result<TValue>> RecoverAsync<TValue>(
        this Result<TValue> result,
        Func<IEnumerable<Error>, Task<Result<TValue>>> recover)
    {
        return result.MatchAsync(
            x => x,
            recover);
    }

    public static Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> task,
        Func<IEnumerable<Error>, Task<Result<T>>> recover)
    {
        return task.MatchAsync(
            x => x,
            recover);
    }

    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> task,
        T fallback)
    {
        var result = await task.ConfigureAwait(false);

        return result.Recover(fallback);
    }
}
