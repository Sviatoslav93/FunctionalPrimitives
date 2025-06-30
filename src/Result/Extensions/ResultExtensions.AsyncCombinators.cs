namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes an async action if the result is successful, returning the original result.
    /// </summary>
    public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> action)
    {
        if (result.IsSuccess)
        {
            await action(result.Value).ConfigureAwait(false);
        }
        return result;
    }

    /// <summary>
    /// Executes an async action if the result is successful, returning the original result.
    /// </summary>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> task, Action<T> action)
    {
        var result = await task.ConfigureAwait(false);
        return result.Tap(action);
    }

    /// <summary>
    /// Executes an async action if the result is successful, returning the original result.
    /// </summary>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> task, Func<T, Task> action)
    {
        var result = await task.ConfigureAwait(false);
        return await result.TapAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes an async action if the result is failed, returning the original result.
    /// </summary>
    public static async Task<Result<T>> TapErrorAsync<T>(this Result<T> result, Func<IEnumerable<Error>, Task> action)
    {
        if (!result.IsSuccess)
        {
            await action(result.Errors).ConfigureAwait(false);
        }
        return result;
    }

    /// <summary>
    /// Executes an async action if the result is failed, returning the original result.
    /// </summary>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> task, Func<IEnumerable<Error>, Task> action)
    {
        var result = await task.ConfigureAwait(false);
        return await result.TapErrorAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Combines multiple async results into a single result.
    /// </summary>
    public static async Task<Result<T[]>> CombineAsync<T>(params Task<Result<T>>[] tasks)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return Combine(results);
    }

    /// <summary>
    /// Combines multiple async results into a single result.
    /// </summary>
    public static async Task<Result<T[]>> CombineAsync<T>(this IEnumerable<Task<Result<T>>> tasks)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return Combine(results);
    }

    /// <summary>
    /// Tries to recover from a failed result by executing an async recovery function.
    /// </summary>
    public static async Task<Result<T>> RecoverAsync<T>(this Result<T> result, Func<IEnumerable<Error>, Task<T>> recovery)
    {
        if (result.IsSuccess)
            return result;

        var recoveredValue = await recovery(result.Errors).ConfigureAwait(false);
        return Result<T>.Success(recoveredValue);
    }

    /// <summary>
    /// Tries to recover from a failed result by executing an async recovery function that returns a Result.
    /// </summary>
    public static async Task<Result<T>> RecoverAsync<T>(this Result<T> result, Func<IEnumerable<Error>, Task<Result<T>>> recovery)
    {
        if (result.IsSuccess)
            return result;

        return await recovery(result.Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// Tries to recover from a failed task result by executing an async recovery function.
    /// </summary>
    public static async Task<Result<T>> RecoverAsync<T>(this Task<Result<T>> task, Func<IEnumerable<Error>, Task<Result<T>>> recovery)
    {
        var result = await task.ConfigureAwait(false);
        return await result.RecoverAsync(recovery).ConfigureAwait(false);
    }
}
