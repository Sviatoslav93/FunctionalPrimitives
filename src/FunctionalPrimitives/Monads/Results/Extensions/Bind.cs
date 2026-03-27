namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms the result of a successful operation into a new result by applying a specified mapping function
    /// or returns the existing errors if the operation was not successful.
    /// </summary>
    public static Result<U> Bind<T, U>(this Result<T> result, Func<T, U> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : Failure<U>(result.ErrorsInternal);
    }

    /// <summary>
    /// Transforms the result of a successful operation into a new result
    /// by applying a specified mapping function or propagates the existing errors.
    /// </summary>
    public static Result<U> Bind<T, U>(this Result<T> result, Func<T, Result<U>> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : Failure<U>(result.ErrorsInternal);
    }

    /// <summary>
    /// Projects the result of a successful operation to another result using LINQ query syntax.
    /// </summary>
    public static Result<U> SelectMany<T, U>(this Result<T> result, Func<T, Result<U>> onSuccess)
    {
        return result.Bind(onSuccess);
    }

    /// <summary>
    /// Projects and flattens the result value using a binder and a result selector, enabling LINQ query syntax.
    /// </summary>
    public static Result<U> SelectMany<T, V, U>(
        this Result<T> result,
        Func<T, Result<V>> binder,
        Func<T, V, U> projector)
    {
        return result.Bind(t =>
            binder(t).Bind(i => Success(projector(t, i))));
    }
}
