namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    /// <summary>
    /// Provides extension methods for performing operations on <see cref="Result{TValue}"/> objects.
    /// </summary>
    extension<TValue>(Result<TValue> result)
    {
        /// <summary>
        /// Transforms a <see cref="Result{TValue}"/> into a value of type <typeparamref name="TNext"/>
        /// using specified functions for success and failure scenarios.
        /// </summary>
        /// <typeparam name="TNext">The type of the result after transformation.</typeparam>
        /// <param name="onSuccess">A function to invoke if the result is successful. The function takes the successful value as a parameter.</param>
        /// <param name="onFailure">A function to invoke if the result is a failure. The function takes the collection of errors as a parameter.</param>
        /// <returns>The transformed value of type <typeparamref name="TNext"/> resulting from invoking either <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</returns>
        public TNext Match<TNext>(
            Func<TValue, TNext> onSuccess,
            Func<IEnumerable<Error>, TNext> onFailure)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Errors);
        }
    }
}
