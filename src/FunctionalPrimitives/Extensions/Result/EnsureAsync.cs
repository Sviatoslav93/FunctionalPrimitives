namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    extension<T>(T value)
    {
        /// <summary>
        /// Asynchronously validates the value using an async predicate.
        /// Returns a successful <see cref="Result{T}"/> containing this value if the predicate resolves to <see langword="true"/>;
        /// otherwise returns a failed result with the specified error.
        /// </summary>
        /// <param name="predicate">An asynchronous function to test the value.</param>
        /// <param name="error">The error to use when the predicate resolves to <see langword="false"/>.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to a successful <see cref="Result{T}"/> if the predicate passes;
        /// otherwise a failed result containing <paramref name="error"/>.
        /// </returns>
        public async Task<Result<T>> EnsureAsync(Func<T, Task<bool>> predicate, Error error)
        {
            return await predicate(value).ConfigureAwait(false) ? value : error;
        }

        /// <summary>
        /// Asynchronously validates the value using an async predicate.
        /// Returns a successful <see cref="Result{T}"/> containing this value if the predicate resolves to <see langword="true"/>;
        /// otherwise returns a failed result produced by the error factory.
        /// </summary>
        /// <param name="predicate">An asynchronous function to test the value.</param>
        /// <param name="error">A factory function that produces the error from the value when the predicate fails.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to a successful <see cref="Result{T}"/> if the predicate passes;
        /// otherwise a failed result containing the error produced by <paramref name="error"/>.
        /// </returns>
        public async Task<Result<T>> EnsureAsync(Func<T, Task<bool>> predicate, Func<T, Error> error)
        {
            return await predicate(value).ConfigureAwait(false) ? value : error(value);
        }
    }

    extension<T>(Task<Result<T>> result)
    {
        /// <summary>
        /// Asynchronously validates the successful value of this result using a synchronous predicate.
        /// If the result is already a failure it is returned unchanged; otherwise the predicate is evaluated
        /// and a failed result with the specified error is returned when it is not satisfied.
        /// </summary>
        /// <param name="predicate">A function to test the successful value.</param>
        /// <param name="error">The error to use when the predicate is not satisfied.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the original result if it is a failure or the predicate passes;
        /// otherwise a failed result containing <paramref name="error"/>.
        /// </returns>
        public Task<Result<T>> EnsureAsync(Func<T, bool> predicate, Error error)
        {
            return result.MatchAsync(
                x => predicate(x) ? Success(x) : error,
                errors => Failure<T>(errors.ToArray()));
        }

        /// <summary>
        /// Asynchronously validates the successful value of this result using a synchronous predicate.
        /// If the result is already a failure it is returned unchanged; otherwise the predicate is evaluated
        /// and a failed result produced by the error factory is returned when it is not satisfied.
        /// </summary>
        /// <param name="predicate">A function to test the successful value.</param>
        /// <param name="error">A factory function that produces the error from the value when the predicate fails.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the original result if it is a failure or the predicate passes;
        /// otherwise a failed result containing the error produced by <paramref name="error"/>.
        /// </returns>
        public Task<Result<T>> EnsureAsync(Func<T, bool> predicate, Func<T, Error> error)
        {
            return result.MatchAsync(
                x => predicate(x) ? Success(x) : error(x),
                errors => Failure<T>(errors.ToArray()));
        }
    }
}
