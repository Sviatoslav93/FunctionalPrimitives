namespace Result.Extensions;

public partial class ResultExtensions
{
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
