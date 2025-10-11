namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Executes an async action if the result is successful, returning the original result.
    /// </summary>
    public static Task<Result<T>> TapAsync<T>(
        this Result<T> result,
        Func<T, Task> action)
    {
        return result.ThenAsync(async x =>
        {
            await action(x).ConfigureAwait(false);

            return x;
        });
    }

    /// <summary>
    /// Executes an async action if the result is successful, returning the original result.
    /// </summary>
    public static async Task<Result<T>> TapAsync<T>(
        this Task<Result<T>> task,
        Action<T> action)
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
}
