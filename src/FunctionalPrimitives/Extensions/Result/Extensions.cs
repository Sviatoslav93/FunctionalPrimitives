namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    extension<T>(T value)
    {
        /// <summary>
        /// Wraps this value in a successful <see cref="Result{T}"/>.
        /// </summary>
        /// <returns>A successful <see cref="Result{T}"/> containing this value.</returns>
        public Result<T> ToResult() => value;
    }

    extension<T>(T? value) where T : class
    {
        /// <summary>
        /// Converts this nullable reference to a <see cref="Result{T}"/>.
        /// Returns a successful result if the value is non-null; otherwise returns a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error to use when the value is <see langword="null"/>.</param>
        /// <returns>
        /// A successful <see cref="Result{T}"/> containing the value, or a failed result with <paramref name="error"/> if null.
        /// </returns>
        public Result<T> ToResult(Error error)
        {
            return value ?? Failure<T>(error);
        }
    }

    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Discards the successful value and maps the result to <see cref="Result{T}"/> of <see cref="Unit"/>.
        /// Useful for ignoring the return value of a successful operation while preserving error propagation.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/> of <see cref="Unit"/> that is successful if the original result was successful.</returns>
        public Result<Unit> Ignore() => result.Map(_ => Unit.Value);
    }

    extension<T>(Task<Result<T>> result)
    {
        /// <summary>
        /// Asynchronously discards the successful value and maps the result to <see cref="Result{T}"/> of <see cref="Unit"/>.
        /// Useful for ignoring the return value of a successful async operation while preserving error propagation.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to a <see cref="Result{T}"/> of <see cref="Unit"/>,
        /// successful if the original result was successful.
        /// </returns>
        public Task<Result<Unit>> IgnoreAsync() => result.MapAsync(_ => Unit.Value);
    }
}
