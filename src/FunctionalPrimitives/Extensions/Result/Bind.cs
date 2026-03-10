namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Transforms the result of a successful operation into a new result by applying a specified mapping function
        /// or returns the existing errors if the operation was not successful.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Result{TNextValue}"/>.</typeparam>
        /// <param name="onSuccess">A function to apply to the value of the successful result.</param>
        /// <returns>
        /// A new <see cref="Result{TNextValue}"/> containing the transformed value if the original result was successful,
        /// or the errors from the original result if it was not successful.
        /// </returns>
        public Result<U> Bind<U>(
            Func<T, U> onSuccess)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : Failure<U>(result.ErrorsInternal);
        }

        /// <summary>
        /// Transforms the result of a successful operation into a new result
        /// by applying a specified mapping function or propagates the existing errors
        /// if the operation was not successful.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Result{TNextValue}"/>.</typeparam>
        /// <param name="onSuccess">A function to apply to the value of the successful result, which returns a new result.</param>
        /// <returns>
        /// A new <see cref="Result{TNextValue}"/> containing the result produced by the mapping function
        /// if the original result was successful, or the errors from the original result if it was not successful.
        /// </returns>
        public Result<U> Bind<U>(
            Func<T, Result<U>> onSuccess)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : Failure<U>(result.ErrorsInternal);
        }

        /// <summary>
        /// Projects the result of a successful operation to another result using LINQ query syntax.
        /// If the original result is a failure, the errors are propagated unchanged.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Result{U}"/>.</typeparam>
        /// <param name="onSuccess">A function that takes the current value and returns a new result.</param>
        /// <returns>
        /// The result produced by <paramref name="onSuccess"/> if the original result was successful,
        /// or the errors from the original result if it was not successful.
        /// </returns>
        public Result<U> SelectMany<U>(
            Func<T, Result<U>> onSuccess)
        {
                return result.Bind(onSuccess);
        }

        /// <summary>
        /// Projects and flattens the result value using a binder and a result selector, enabling LINQ query syntax.
        /// If any step fails, the corresponding errors are propagated.
        /// </summary>
        /// <typeparam name="V">The type of the intermediate value returned by the binder function.</typeparam>
        /// <typeparam name="U">The type of the final value produced by the selector function.</typeparam>
        /// <param name="binder">A function applied to the current value that returns an intermediate result.</param>
        /// <param name="projector">A function that combines the original and intermediate values into the final value.</param>
        /// <returns>
        /// A <see cref="Result{U}"/> containing the projected value if all operations were successful;
        /// otherwise, the errors from the first failed step.
        /// </returns>
        public Result<U> SelectMany<V, U>(
            Func<T, Result<V>> binder,
            Func<T, V, U> projector)
        {
            return result.Bind(t =>
                binder(t).Bind(i => Success(projector(t, i))));
        }
    }
}
