namespace Result.Extensions;

public static partial class ResultExtensions
{
    extension<TValue>(Result<TValue> result)
    {
        /// <summary>
        /// Transforms the result of a successful operation into a new result by applying a specified mapping function
        /// or returns the existing errors if the operation was not successful.
        /// </summary>
        /// <typeparam name="TNextValue">The type of the value in the resulting <see cref="Result{TNextValue}"/>.</typeparam>
        /// <param name="onSuccess">A function to apply to the value of the successful result.</param>
        /// <returns>
        /// A new <see cref="Result{TNextValue}"/> containing the transformed value if the original result was successful,
        /// or the errors from the original result if it was not successful.
        /// </returns>
        public Result<TNextValue> Bind<TNextValue>(
            Func<TValue, TNextValue> onSuccess)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : result.Errors;
        }

        /// <summary>
        /// Transforms the result of a successful operation into a new result
        /// by applying a specified mapping function or propagates the existing errors
        /// if the operation was not successful.
        /// </summary>
        /// <typeparam name="TNextValue">The type of the value in the resulting <see cref="Result{TNextValue}"/>.</typeparam>
        /// <param name="onSuccess">A function to apply to the value of the successful result, which returns a new result.</param>
        /// <returns>
        /// A new <see cref="Result{TNextValue}"/> containing the result produced by the mapping function
        /// if the original result was successful, or the errors from the original result if it was not successful.
        /// </returns>
        public Result<TNextValue> Bind<TNextValue>(
            Func<TValue, Result<TNextValue>> onSuccess)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : result.Errors;
        }
    }
}
