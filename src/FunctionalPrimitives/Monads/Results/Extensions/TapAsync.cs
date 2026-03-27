using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    public static Task<Result<T>> TapAsync<T>(
        this Result<T> result,
        Func<T, Task> action)
    {
        return result.BindAsync(async x =>
        {
            await action(x).ConfigureAwait(false);

            return x;
        });
    }

    public static async Task<Result<T>> TapErrorAsync<T>(
        this Result<T> result,
        Func<IEnumerable<Error>, Task> action)
    {
        if (!result.IsSuccess)
        {
            await action(result.Errors).ConfigureAwait(false);
        }

        return result;
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(
        this Task<Result<TValue>> task,
        Action<TValue> action)
    {
        var result = await task.ConfigureAwait(false);
        return result.Tap(action);
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Task> action)
    {
        var result = await task.ConfigureAwait(false);
        return await result.TapAsync(action).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> TapErrorAsync<TValue>(
        this Task<Result<TValue>> task,
        Func<IEnumerable<Error>, Task> action)
    {
        var result = await task.ConfigureAwait(false);
        return await result.TapErrorAsync(action).ConfigureAwait(false);
    }

    public static async Task<Result<TValue>> TapErrorAsync<TValue>(
        this Task<Result<TValue>> task,
        Action action)
    {
        var result = await task.ConfigureAwait(false);
        return result.TapError(action);
    }
}
