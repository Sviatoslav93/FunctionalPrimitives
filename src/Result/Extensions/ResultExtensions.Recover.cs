namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Try to recover from a failed result by providing a fallback value.
    /// </summary>
    /// <returns> fallback value. </returns>
    public static Result<T> Recover<T>(
        this Result<T> result,
        T fallbackValue)
    {
        return result.Match(
            x => x,
            _ => Result.Success(fallbackValue));
    }

    /// <summary>
    /// Try to recover from a failed result by executing a recovery function.
    /// </summary>
    /// <returns> fallback value from the recovery factory. </returns>
    public static Result<T> Recover<T>(
        this Result<T> result,
        Func<IEnumerable<Error>, T> recoveryFactory)
    {
        return result.Match(
            x => x,
            err => Result.Success(recoveryFactory(err)));
    }
}
