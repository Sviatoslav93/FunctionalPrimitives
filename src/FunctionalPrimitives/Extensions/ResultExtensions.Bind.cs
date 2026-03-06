namespace FunctionalPrimitives.Extensions;

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
                : Failure<U>(result.Errors);
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
                : Failure<U>(result.Errors);
        }

        public Result<U> SelectMany<U>(
            Func<T, Result<U>> onSuccess)
        {
                return result.Bind(onSuccess);
        }

        public Result<U> SelectMany<V, U>(
            Func<T, Result<V>> binder,
            Func<T, V, U> projector)
        {
            return result.Bind(t =>
                binder(t).Bind(i => Success(projector(t, i))));
        }
    }
}
