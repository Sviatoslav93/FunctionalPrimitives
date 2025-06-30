namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines multiple results into a single result containing all values or all errors.
    /// </summary>
    public static Result<T[]> Combine<T>(params Result<T>[] results)
    {
        var errors = new List<Error>();
        var values = new List<T>();

        foreach (var result in results)
        {
            if (result.IsSuccess)
            {
                values.Add(result.Value);
            }
            else
            {
                errors.AddRange(result.Errors);
            }
        }

        return errors.Count > 0
            ? Result<T[]>.Failed(errors)
            : Result<T[]>.Success(values.ToArray());
    }

    /// <summary>
    /// Combines multiple results into a single result containing all values or all errors.
    /// </summary>
    public static Result<T[]> Combine<T>(this IEnumerable<Result<T>> results)
    {
        return Combine(results.ToArray());
    }

    /// <summary>
    /// Executes an action if the result is successful, returning the original result.
    /// </summary>
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }
        return result;
    }

    /// <summary>
    /// Executes an action if the result is failed, returning the original result.
    /// </summary>
    public static Result<T> TapError<T>(this Result<T> result, Action<IEnumerable<Error>> action)
    {
        if (!result.IsSuccess)
        {
            action(result.Errors);
        }
        return result;
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

    /// <summary>
    /// Tries to recover from a failed result by providing a fallback value.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, T fallbackValue)
    {
        return result.IsSuccess ? result : Result<T>.Success(fallbackValue);
    }

    /// <summary>
    /// Tries to recover from a failed result by executing a recovery function.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, Func<IEnumerable<Error>, T> recovery)
    {
        return result.IsSuccess ? result : Result<T>.Success(recovery(result.Errors));
    }

    /// <summary>
    /// Tries to recover from a failed result by executing a recovery function that returns a Result.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, Func<IEnumerable<Error>, Result<T>> recovery)
    {
        return result.IsSuccess ? result : recovery(result.Errors);
    }
}
