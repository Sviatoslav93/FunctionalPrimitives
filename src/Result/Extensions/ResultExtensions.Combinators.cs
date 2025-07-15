namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines multiple results into a single result containing all values or all errors.
    /// </summary>
    public static Result<T[]> Combine<T>(this IEnumerable<Result<T>> results)
    {
        return Combine(results.ToArray());
    }

    /// <summary>
    /// Filters the result based on a predicate.
    /// </summary>
    public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (!result.IsSuccess)
            return result;

        return predicate(result.Value) ? result : error;
    }

    /// <summary>
    /// Filters the result based on a predicate.
    /// </summary>
    public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate, Func<T, Error> errorFactory)
    {
        if (!result.IsSuccess)
            return result;

        return predicate(result.Value) ? result : errorFactory(result.Value);
    }
}
