namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Executes an asynchronous action on the value of a successful result.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The result to act upon. If the result is not successful, the action is not executed.</param>
    /// <param name="action">The asynchronous action to execute on the value of the result.</param>
    /// <returns>A task representing the result, retaining its original state unless the action modifies it.</returns>
    public static Task<Result<T>> DoAsync<T>(
        this Result<T> result,
        Func<T, Task> action)
    {
        return result.BindAsync(async x =>
        {
            await action(x).ConfigureAwait(false);

            return x;
        });
    }

    /// <summary>
    /// Executes a synchronous action on the value of a successful result when the result is represented as a task.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="task">A task representing the asynchronous result operation.</param>
    /// <param name="action">The synchronous action to execute on the value of the result if it is successful.</param>
    /// <returns>A task containing the result of the operation, retaining its original state unless the action modifies it.</returns>
    public static async Task<Result<T>> DoAsync<T>(
        this Task<Result<T>> task,
        Action<T> action)
    {
        var result = await task.ConfigureAwait(false);
        return result.Do(action);
    }

    /// <summary>
    /// Executes an asynchronous action on the value of a successful result contained within a task.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="task">A task that represents a result object. If the task represents a failed result, the action is not executed.</param>
    /// <param name="action">The asynchronous action to execute on the value of the successful result.</param>
    /// <returns>A task that represents the result, retaining its original state unless the action modifies it.</returns>
    public static async Task<Result<T>> DoAsync<T>(
        this Task<Result<T>> task,
        Func<T, Task> action)
    {
        var result = await task.ConfigureAwait(false);
        return await result.DoAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes an asynchronous action on the errors of a failed result.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The result to act upon. If the result is successful, the action is not executed.</param>
    /// <param name="action">The asynchronous action to execute on the errors of the result.</param>
    /// <returns>A task representing the result, retaining its original state regardless of the action execution.</returns>
    public static async Task<Result<T>> DoErrorAsync<T>(
        this Result<T> result,
        Func<IEnumerable<Error>, Task> action)
    {
        if (!result.IsSuccess)
        {
            await action(result.Errors).ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Executes an asynchronous action on the errors of a failed result, if any.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="task">The result task to act upon. If the result is successful, the action is not executed.</param>
    /// <param name="action">The asynchronous action to execute on the errors of the result.</param>
    /// <returns>A task representing the result, retaining its original state unless the action modifies it.</returns>
    public static async Task<Result<T>> DoErrorAsync<T>(
        this Task<Result<T>> task,
        Func<IEnumerable<Error>, Task> action)
    {
        var result = await task.ConfigureAwait(false);
        return await result.DoErrorAsync(action).ConfigureAwait(false);
    }
}
