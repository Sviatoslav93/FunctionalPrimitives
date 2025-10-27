namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Asynchronously transforms the value of a successful result using the provided function.
    /// If the result represents a failure, the original errors are propagated.
    /// </summary>
    /// <param name="result">The result to be transformed.</param>
    /// <param name="onSuccess">A function that takes the value of the result and returns a task
    /// that produces a transformed value of the next result type.</param>
    /// <typeparam name="TValue">The type of the successful value in the current result.</typeparam>
    /// <typeparam name="TNextValue">The type of the successful value in the transformed result.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains
    /// the transformed result if the input result is successful, otherwise the original errors are returned.</returns>
    public static async Task<Result<TNextValue>> BindAsync<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue,
        Task<TNextValue>> onSuccess)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : result.Errors;
    }

    /// <summary>
    /// Asynchronously transforms the value of a successful result using the provided asynchronous function.
    /// If the result represents a failure, the original errors are propagated.
    /// </summary>
    /// <param name="result">The input result to be processed.</param>
    /// <param name="onSuccess">A function that takes the value of the result and returns a task
    /// producing a transformed result.</param>
    /// <typeparam name="TValue">The type of the successful value in the input result.</typeparam>
    /// <typeparam name="TNextValue">The type of the successful value in the output result.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains the transformed
    /// result if the input result is successful, otherwise the original errors are returned.</returns>
    public static async Task<Result<TNextValue>> BindAsync<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, Task<Result<TNextValue>>> onSuccess)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : result.Errors;
    }

    /// <summary>
    /// Asynchronously transforms the value of a successful result from a task using the provided function.
    /// If the result represents a failure, the original errors are propagated.
    /// </summary>
    /// <param name="task">The task representing a result to be transformed.</param>
    /// <param name="onSuccess">A function that takes the value of the result and returns a transformed value of the next result type.</param>
    /// <typeparam name="TValue">The type of the successful value in the current result.</typeparam>
    /// <typeparam name="TNextValue">The type of the successful value in the transformed result.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains the transformed result if the input result is successful, otherwise the original errors are returned.</returns>
    public static async Task<Result<TNextValue>> BindAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, TNextValue> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return result.Bind(onSuccess);
    }

    /// <summary>
    /// Asynchronously transforms the value of a successful result using the provided function.
    /// If the result represents a failure, the original errors are propagated.
    /// </summary>
    /// <param name="task">The task representing a result to be transformed.</param>
    /// <param name="onSuccess">A function that takes the successful value of the result and returns a new result synchronously.</param>
    /// <typeparam name="TValue">The type of the value in the original result.</typeparam>
    /// <typeparam name="TNextValue">The type of the value in the transformed result.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains
    /// the transformed result if the input result is successful, otherwise the original errors are returned.</returns>
    public static async Task<Result<TNextValue>> BindAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Result<TNextValue>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return result.Bind(onSuccess);
    }

    /// <summary>
    /// Asynchronously transforms the value of a successful result encapsulated in a task
    /// using the provided asynchronous function. If the result indicates a failure, the
    /// original errors are propagated without invoking the transformation function.
    /// </summary>
    /// <param name="task">A task representing an asynchronous operation that produces a result
    /// to be transformed.</param>
    /// <param name="onSuccess">A function to be invoked if the result is successful. This function
    /// takes the successful value as input and returns a task that produces a transformed
    /// value of the next result type.</param>
    /// <typeparam name="TValue">The type of the value in the initial successful result.</typeparam>
    /// <typeparam name="TNextValue">The type of the value in the transformed result.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains
    /// the transformed result if the input result encapsulated in the task is successful;
    /// otherwise, the original errors are returned.</returns>
    public static async Task<Result<TNextValue>> BindAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNextValue>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.BindAsync(onSuccess);
    }

    /// <summary>
    /// Asynchronously transforms the value of a successful result represented by a task, using the provided function.
    /// If the result represents a failure, the original errors are propagated.
    /// </summary>
    /// <param name="task">The task producing the result to be transformed.</param>
    /// <param name="onSuccess">A function that takes the value of the result and returns a task
    /// that produces another result with a transformed value.</param>
    /// <typeparam name="TValue">The type of the successful value in the current result.</typeparam>
    /// <typeparam name="TNextValue">The type of the successful value in the transformed result.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains the transformed result if the input result is successful, otherwise the original errors are returned.</returns>
    public static async Task<Result<TNextValue>> BindAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<Result<TNextValue>>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.BindAsync(onSuccess);
    }
}
