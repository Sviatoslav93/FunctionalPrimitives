using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    public static async Task<U> MatchAsync<T, U>(
        this Result<T> result,
        Func<T, Task<U>> onSuccess,
        Func<IEnumerable<Error>, Task<U>> onFailure)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }

    public static async Task<U> MatchAsync<T, U>(
        this Result<T> result,
        Func<T, Task<U>> onSuccess,
        Func<IEnumerable<Error>, U> onFailure)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : onFailure(result.Errors);
    }

    public static async Task<U> MatchAsync<T, U>(
        this Result<T> result,
        Func<T, U> onSuccess,
        Func<IEnumerable<Error>, Task<U>> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }

    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, TNext> onSuccess,
        Func<IReadOnlyList<Error>, TNext> onFailure)
    {
        var result = await task.ConfigureAwait(false);
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Errors);
    }

    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNext>> onSuccess,
        Func<IEnumerable<Error>, TNext> onFailure)
    {
        var result = await task.ConfigureAwait(false);
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : onFailure(result.Errors);
    }

    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, TNext> onSuccess,
        Func<IEnumerable<Error>, Task<TNext>> onFailure)
    {
        var result = await task.ConfigureAwait(false);
        return result.IsSuccess
            ? onSuccess(result.Value)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }

    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNext>> onSuccess,
        Func<IEnumerable<Error>, Task<TNext>> onFailure)
    {
        var result = await task.ConfigureAwait(false);
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }
}
