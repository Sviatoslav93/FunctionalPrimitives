using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Wraps this value in a successful <see cref="Result{T}"/>.
    /// </summary>
    public static Result<T> ToResult<T>(this T value) => value;

    /// <summary>
    /// Converts this nullable reference to a <see cref="Result{T}"/>.
    /// Returns a successful result if the value is non-null; otherwise returns a failed result with the specified error.
    /// </summary>
    public static Result<T> ToResult<T>(this T? value, Error error)
        where T : class
    {
        return value ?? Failure<T>(error);
    }

    /// <summary>
    /// Discards the successful value and maps the result to <see cref="Result{T}"/> of <see cref="Unit"/>.
    /// </summary>
    public static Result<Unit> Ignore<T>(this Result<T> result) => result.Map(_ => Unit.Value);

    /// <summary>
    /// Asynchronously discards the successful value and maps the result to <see cref="Result{T}"/> of <see cref="Unit"/>.
    /// </summary>
    public static Task<Result<Unit>> IgnoreAsync<T>(this Task<Result<T>> result) => result.MapAsync(_ => Unit.Value);
}
