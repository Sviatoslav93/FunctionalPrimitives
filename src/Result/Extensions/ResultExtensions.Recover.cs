namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Recovers a failed result by providing a fallback value to use in case of failure.
    /// </summary>
    /// <param name="result">The result to recover.</param>
    /// <param name="fallbackValue">The fallback value to return if the result is a failure.</param>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <returns>
    /// A successful result containing either the original value if the result was successful,
    /// or the fallback value if the result was a failure.
    /// </returns>
    public static Result<T> Recover<T>(
        this Result<T> result,
        T fallbackValue)
    {
        return result.Match(
            x => x,
            _ => Result.Success(fallbackValue));
    }

    /// <summary>
    /// Recovers a failed result by using a factory function to generate a fallback value
    /// based on the errors associated with the failure.
    /// </summary>
    /// <param name="result">The result to recover.</param>
    /// <param name="recoveryFactory">
    /// A factory function that takes the collection of errors and returns a fallback value
    /// to use in case of failure.
    /// </param>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <returns>
    /// A successful result containing either the original value if the result was successful,
    /// or the value produced by the factory function if the result was a failure.
    /// </returns>
    public static Result<T> Recover<T>(
        this Result<T> result,
        Func<IEnumerable<Error>, T> recoveryFactory)
    {
        return result.Match(
            x => x,
            err => Result.Success(recoveryFactory(err)));
    }
}
