namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    extension<T>(T value)
    {
        /// <summary>
        /// Returns a successful <see cref="Result{TValue}"/> containing this value if the predicate is satisfied;
        /// otherwise returns a failed result with the specified error.
        /// </summary>
        /// <param name="predicate">A function to test the value.</param>
        /// <param name="error">The error to use when the predicate is not satisfied.</param>
        /// <returns>
        /// A successful <see cref="Result{TValue}"/> if <paramref name="predicate"/> returns <see langword="true"/>;
        /// otherwise a failed result containing <paramref name="error"/>.
        /// </returns>
        public Result<T> Ensure(Func<T, bool> predicate, Error error)
        {
            return predicate(value) ? value : error;
        }
    }

    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Returns the original result if it is already a failure; otherwise evaluates the predicate against
        /// the successful value and returns a failed result with the specified error when the predicate is not satisfied.
        /// </summary>
        /// <param name="predicate">A function to test the successful value.</param>
        /// <param name="error">The error to use when the predicate is not satisfied.</param>
        /// <returns>
        /// The original result when it is a failure or when <paramref name="predicate"/> returns <see langword="true"/>;
        /// otherwise a failed result containing <paramref name="error"/>.
        /// </returns>
        public Result<T> Ensure(Func<T, bool> predicate, Error error)
        {
            if (result.IsFailure) return result;

            return predicate(result.Value) ? result : error;
        }
    }
}
