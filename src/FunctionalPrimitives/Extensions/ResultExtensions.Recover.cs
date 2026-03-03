namespace FunctionalPrimitives.Extensions;

public partial class ResultExtensions
{
    /// <param name="result">The result to recover.</param>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    extension<TValue>(Result<TValue> result)
    {
        /// <summary>
        /// Recovers a failed result by providing a fallback value to use in case of failure.
        /// </summary>
        /// <param name="fallbackValue">The fallback value to return if the result is a failure.</param>
        /// <returns>
        /// A successful result containing either the original value if the result was successful,
        /// or the fallback value if the result was a failure.
        /// </returns>
        public Result<TValue> Recover(TValue fallbackValue)
        {
            return result.Match(
                x => x,
                _ => FunctionalPrimitives.Result.Success(fallbackValue));
        }

        /// <summary>
        /// Recovers a failed result by using a factory function to generate a fallback value
        /// based on the errors associated with the failure.
        /// </summary>
        /// <param name="recoveryFactory">
        /// A factory function that takes the collection of errors and returns a fallback value
        /// to use in case of failure.
        /// </param>
        /// <returns>
        /// A successful result containing either the original value if the result was successful,
        /// or the value produced by the factory function if the result was a failure.
        /// </returns>
        public Result<TValue> Recover(Func<IEnumerable<Error>, TValue> recoveryFactory)
        {
            return result.Match(
                x => x,
                err => FunctionalPrimitives.Result.Success(recoveryFactory(err)));
        }
    }
}
