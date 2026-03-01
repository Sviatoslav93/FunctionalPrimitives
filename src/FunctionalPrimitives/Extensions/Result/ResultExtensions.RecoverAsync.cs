namespace FunctionalPrimitives.Extensions.Result;

public partial class ResultExtensions
{
    /// <param name="result">The initial result that may represent a success or failure state.</param>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    extension<TValue>(Result<TValue> result)
    {
        /// <summary>
        /// Attempts to recover from a failure result by asynchronously invoking a recovery function
        /// and returning a new result based on the recovered value.
        /// </summary>
        /// <param name="recovery">
        /// A function that will asynchronously process the collection of errors from the failure result
        /// and provide a recovery value of type <typeparamref name="TValue"/>.
        /// </param>
        /// <returns>
        /// A new result of type <typeparamref name="TValue"/>.
        /// If the original result is a success, the same result is returned.
        /// If the original result is a failure, a new result containing the recovery value is returned.
        /// </returns>
        public async Task<Result<TValue>> RecoverAsync(Func<IEnumerable<Error>, Task<TValue>> recovery)
        {
            if (result.IsSuccess)
                return result;

            var recoveredValue = await recovery(result.Errors).ConfigureAwait(false);
            return FunctionalPrimitives.Result.Success(recoveredValue);
        }

        /// <summary>
        /// Attempts to recover from a failure result by asynchronously invoking a recovery function
        /// that returns a new result.
        /// </summary>
        /// <param name="recovery">
        /// A function that will asynchronously process the collection of errors from the failure result
        /// and provide a new result of type <typeparamref name="TValue"/>.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation that yields a result of type <typeparamref name="TValue"/>.
        /// If the original result is a success, the same result is returned.
        /// If the original result is a failure, the result returned by the recovery function is returned.
        /// </returns>
        public Task<Result<TValue>> RecoverAsync(Func<IEnumerable<Error>, Task<Result<TValue>>> recovery)
        {
            return result.MatchAsync(
                x => x,
                recovery);
        }
    }

    /// <param name="task">A task representing the asynchronous operation that produces the initial result.</param>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    extension<T>(Task<Result<T>> task)
    {
        /// <summary>
        /// Attempts to recover from a failure result by asynchronously invoking a recovery function
        /// that returns a new result. This overload accepts a task that produces the result.
        /// </summary>
        /// <param name="recovery">
        /// A function that will asynchronously process the collection of errors from the failure result
        /// and provide a new result of type <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation that yields a result of type <typeparamref name="T"/>.
        /// If the original result is a success, the same result is returned.
        /// If the original result is a failure, the result returned by the recovery function is returned.
        /// </returns>
        public Task<Result<T>> RecoverAsync(Func<IEnumerable<Error>, Task<Result<T>>> recovery)
        {
            return task.MatchAsync(
                x => x,
                recovery);
        }

        /// <summary>
        /// Attempts to recover from a failure result by providing a fallback value.
        /// This overload accepts a task that produces the result.
        /// </summary>
        /// <param name="fallback">The fallback value to use if the result is a failure.</param>
        /// <returns>
        /// A task representing the asynchronous operation that yields a result of type <typeparamref name="T"/>.
        /// If the original result is a success, the same result is returned.
        /// If the original result is a failure, a new success result containing the fallback value is returned.
        /// </returns>
        public async Task<Result<T>> RecoverAsync(T fallback)
        {
            var result = await task.ConfigureAwait(false);

            return result.Recover(fallback);
        }
    }
}
