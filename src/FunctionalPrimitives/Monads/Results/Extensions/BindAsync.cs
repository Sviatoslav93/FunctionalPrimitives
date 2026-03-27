namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result<U>> BindAsync<T, U>(
        this Result<T> result,
        Func<T, Task<U>> onSuccess)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : Failure<U>(result.ErrorsInternal);
    }

    public static async Task<Result<U>> BindAsync<T, U>(
        this Result<T> result,
        Func<T, Task<Result<U>>> onSuccess)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : Failure<U>(result.ErrorsInternal);
    }

    public static async Task<Result<U>> SelectMany<T, U>(
        this Result<T> result,
        Func<T, Task<Result<U>>> onSuccess)
    {
        return await result.BindAsync(onSuccess).ConfigureAwait(false);
    }

    public static Task<Result<U>> SelectMany<T, V, U>(
        this Result<T> result,
        Func<T, Task<Result<V>>> binder,
        Func<T, V, U> projector)
    {
        return result.BindAsync(binder)
            .MapAsync(v => projector(result.Value, v));
    }

    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> task,
        Func<T, U> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return result.Bind(onSuccess);
    }

    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> task,
        Func<T, Result<U>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return result.Bind(onSuccess);
    }

    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> task,
        Func<T, Task<U>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.BindAsync(onSuccess);
    }

    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> task,
        Func<T, Task<Result<U>>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.BindAsync(onSuccess);
    }

    public static async Task<Result<U>> SelectMany<T, U>(
        this Task<Result<T>> task,
        Func<T, Task<Result<U>>> onSuccess)
    {
        var result = await task.ConfigureAwait(false);
        return await result.SelectMany(onSuccess).ConfigureAwait(false);
    }

    public static async Task<Result<U>> SelectMany<T, V, U>(
        this Task<Result<T>> task,
        Func<T, Task<Result<V>>> binder,
        Func<T, V, U> projector)
    {
        var result = await task.ConfigureAwait(false);
        return await result.SelectMany(binder, projector).ConfigureAwait(false);
    }
}
