namespace FunctionalPrimitives.Extensions;

public static partial class ResultExtensions
{
    extension<TValue>(Result<TValue> result)
    {
        /// <summary>
        /// Transforms the value of a successful result using the specified mapping function.
        /// If the result is a failure, the errors are propagated unchanged.
        /// </summary>
        /// <typeparam name="TNext">The type of the value in the resulting <see cref="Result{TNext}"/>.</typeparam>
        /// <param name="mapper">A function applied to the successful value to produce a new value.</param>
        /// <returns>
        /// A <see cref="Result{TNext}"/> containing the mapped value if the original result was successful;
        /// otherwise, the errors from the original result.
        /// </returns>
        public Result<TNext> Map<TNext>(Func<TValue, TNext> mapper)
        {
            return result.IsSuccess
                ? mapper(result.Value)
                : result.Errors.ToArray();
        }

        /// <summary>
        /// Projects the value of a successful result using LINQ query syntax.
        /// </summary>
        /// <typeparam name="TNext">The type of the projected value.</typeparam>
        /// <param name="selector">The projection function applied to the successful value.</param>
        /// <returns>The same result as calling <c>Map(selector)</c>.</returns>
        public Result<TNext> Select<TNext>(
            Func<TValue, TNext> selector)
            => result.Map(selector);
    }
}
