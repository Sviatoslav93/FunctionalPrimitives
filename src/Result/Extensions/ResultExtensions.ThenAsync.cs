namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Executes a function if the result is successful, returning the original result.
    /// </summary>
    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue,
        Task<TNextValue>> onSuccess)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : result.Errors;
    }

    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, Task<Result<TNextValue>>> onSuccess)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : result.Errors;
    }

    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, TNextValue> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return result.Then(onSuccess);
    }

    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Result<TNextValue>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return result.Then(onSuccess);
    }

    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNextValue>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.ThenAsync(onSuccess);
    }

    public static async Task<Result<TNextValue>> ThenAsync<TValue, TNextValue>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<Result<TNextValue>>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.ThenAsync(onSuccess);
    }
}
