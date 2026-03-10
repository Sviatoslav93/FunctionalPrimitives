namespace FunctionalPrimitives.Extensions.Result;

public partial class ResultExtensions
{
    /// <param name="result">The result to recover.</param>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Recovers a failed result by providing a fallback value to use in case of failure.
        /// </summary>
        /// <param name="fallback">The fallback value to return if the result is a failure.</param>
        /// <returns>
        /// A successful result containing either the original value if the result was successful,
        /// or the fallback value if the result was a failure.
        /// </returns>
        public Result<T> Recover(T fallback)
        {
            return result.Match(
                x => x,
                _ => Success(fallback));
        }

        /// <summary>
        /// Recovers a failed result by using a factory function to generate a fallback value
        /// based on the errors associated with the failure.
        /// </summary>
        /// <param name="recover">
        /// A factory function that takes the collection of errors and returns a fallback value
        /// to use in case of failure.
        /// </param>
        /// <returns>
        /// A successful result containing either the original value if the result was successful,
        /// or the value produced by the factory function if the result was a failure.
        /// </returns>
        public Result<T> Recover(Func<IEnumerable<Error>, T> recover)
        {
            return result.Match(
                x => x,
                err => Success(recover(err)));
        }
    }
}
