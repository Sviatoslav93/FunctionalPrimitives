using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Returns a successful <see cref="Result{TValue}"/> containing this value if the predicate is satisfied;
    /// otherwise returns a failed result with the specified error.
    /// </summary>
    public static Result<T> Ensure<T>(this T value, Func<T, bool> predicate, Error error)
    {
        return predicate(value) ? value : error;
    }

    /// <summary>
    /// Returns the original result if it is already a failure; otherwise evaluates the predicate against
    /// the successful value and returns a failed result with the specified error when the predicate is not satisfied.
    /// </summary>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure) return result;

        return predicate(result.Value) ? result : error;
    }
}
