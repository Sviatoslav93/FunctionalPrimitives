namespace FunctionalPrimitives.Monads.Results.Extensions;

public partial class ResultExtensions
{
    extension<T>(Task<Result<T>> resultTask)
    {
        /// <summary>
        /// Asynchronously transforms the value of a successful result using the specified mapping function.
        /// If the result is a failure, the errors are propagated unchanged.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Result{U}"/>.</typeparam>
        /// <param name="projection">A function applied to the successful value to produce a new value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Result{U}"/> containing the mapped value
        /// if the original result was successful; otherwise, the errors from the original result.
        /// </returns>
        public async Task<Result<U>> MapAsync<U>(Func<T, U> projection)
        {
             var result = await resultTask.ConfigureAwait(false);

             return result.IsSuccess
                ? projection(result.Value)
                : Failure<U>(result.ErrorsInternal);
        }

        /// <summary>
        /// Asynchronously projects the value of a successful result using LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="projection">The projection function applied to the successful value.</param>
        /// <returns>The same result as calling <c>MapAsync(projection)</c>.</returns>
        public Task<Result<U>> Select<U>(Func<T, U> projection)
            => resultTask.MapAsync(projection);
    }
}
