namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Tries to recover from a failed result by providing a fallback value.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, T fallbackValue)
    {
        return result.IsSuccess ? result : Result.Success(fallbackValue);
    }

    /// <summary>
    /// Tries to recover from a failed result by executing a recovery function.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, Func<IEnumerable<Error>, T> recovery)
    {
        return result.IsSuccess ? result : Result.Success(recovery(result.Errors));
    }

    /// <summary>
    /// Tries to recover from a failed result by executing a recovery function that returns a Result.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, Func<IEnumerable<Error>, Result<T>> recovery)
    {
        return result.IsSuccess ? result : recovery(result.Errors);
    }
}
