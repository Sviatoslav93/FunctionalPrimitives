namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Transforms the value of a successful result using the specified mapping function.
        /// If the result is a failure, the errors are propagated unchanged.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Result{TNext}"/>.</typeparam>
        /// <param name="mapper">A function applied to the successful value to produce a new value.</param>
        /// <returns>
        /// A <see cref="Result{TNext}"/> containing the mapped value if the original result was successful;
        /// otherwise, the errors from the original result.
        /// </returns>
        public Result<U> Map<U>(Func<T, U> mapper)
        {
            return result.IsSuccess
                ? mapper(result.Value)
                : Failure<U>(result.ErrorsInternal);
        }

        /// <summary>
        /// Projects the value of a successful result using LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="selector">The projection function applied to the successful value.</param>
        /// <returns>The same result as calling <c>Map(selector)</c>.</returns>
        public Result<U> Select<U>(
            Func<T, U> selector)
            => result.Map(selector);
    }
}
