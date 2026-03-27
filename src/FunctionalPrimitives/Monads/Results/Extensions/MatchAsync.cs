using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <param name="result">The result to be processed. Can be in a success or failure state.</param>
    /// <typeparam name="T">The type of the value encapsulated in the result when successful.</typeparam>
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Matches the result asynchronously by executing the provided success or failure functions
        /// based on the state of the result.
        /// </summary>
        /// <param name="onSuccess">A function to execute when the result is successful. The function takes the success value of type <typeparamref name="T"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <param name="onFailure">A function to execute when the result is a failure. The function takes the collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <typeparam name="U">The type of the value to be returned by the functions.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed function.</returns>
        public async Task<U> MatchAsync<U>(
            Func<T, Task<U>> onSuccess,
            Func<IEnumerable<Error>, Task<U>> onFailure)
        {
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : await onFailure(result.Errors).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes the result asynchronously by executing the provided success or failure functions
        /// based on the state of the result.
        /// </summary>
        /// <param name="onSuccess">A function invoked when the result is successful. This function receives the success value of type <typeparamref name="T"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <param name="onFailure">A function invoked when the result is a failure. This function receives the collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <typeparam name="U">The type of the value to be returned by the executed functions.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> representing the output of either the success or failure function, depending on the result's state.</returns>
        public async Task<U> MatchAsync<U>(
            Func<T, Task<U>> onSuccess,
            Func<IEnumerable<Error>, U> onFailure)
        {
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : onFailure(result.Errors);
        }

        /// <summary>
        /// Processes a <see cref="Result{TValue}"/> by executing the provided success or failure functions
        /// based on its state, and returns a value of type <typeparamref name="U"/>.
        /// </summary>
        /// <param name="onSuccess">A function to execute if the result is successful. It takes the success value of type <typeparamref name="T"/> and returns a value of type <typeparamref name="U"/>.</param>
        /// <param name="onFailure">A function to execute if the result is in a failure state. It takes a collection of <see cref="Error"/> and returns a value of type <typeparamref name="U"/> asynchronously as a <see cref="Task{TNext}"/>.</param>
        /// <typeparam name="U">The type of the value that the provided functions return.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> representing the result of the invoked success or failure function.</returns>
        public async Task<U> MatchAsync<U>(
            Func<T, U> onSuccess,
            Func<IEnumerable<Error>, Task<U>> onFailure)
        {
            return result.IsSuccess
                ? onSuccess(result.Value)
                : await onFailure(result.Errors).ConfigureAwait(false);
        }
    }

    /// <param name="task">A task containing the result to be processed. The result can either be successful or a failure.</param>
    /// <typeparam name="TValue">The type of the value encapsulated in the successful result.</typeparam>
    extension<TValue>(Task<Result<TValue>> task)
    {
        /// <summary>
        /// Processes a task containing a result either by invoking the success function with the value from the result
        /// or the failure function with the collection of errors and returns the computed value.
        /// </summary>
        /// <param name="onSuccess">A function to execute when the result is successful. Takes the success value of type <typeparamref name="TValue"/> and returns a value of type <typeparamref name="TNext"/>.</param>
        /// <param name="onFailure">A function to execute when the result is a failure. Takes a collection of <see cref="Error"/> and returns a value of type <typeparamref name="TNext"/>.</param>
        /// <typeparam name="TNext">The type of the value to be returned by the success or failure functions.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> that represents the result of applying either the success or failure function.</returns>
        public async Task<TNext> MatchAsync<TNext>(
            Func<TValue, TNext> onSuccess,
            Func<IReadOnlyList<Error>, TNext> onFailure)
        {
            var result = await task.ConfigureAwait(false);
            return result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Errors);
        }

        /// <summary>
        /// Matches the result asynchronously by executing the provided success or failure functions
        /// based on the state of the task containing the result.
        /// </summary>
        /// <param name="onSuccess">A function to execute when the result is successful. The function takes the success value of type <typeparamref name="TValue"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <param name="onFailure">A function to execute when the result is a failure. The function takes the collection of <see cref="Error"/> and returns a value of type <typeparamref name="TNext"/>.</param>
        /// <typeparam name="TNext">The type of the value to be returned by the functions.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed function.</returns>
        public async Task<TNext> MatchAsync<TNext>(
            Func<TValue, Task<TNext>> onSuccess,
            Func<IEnumerable<Error>, TNext> onFailure)
        {
            var result = await task.ConfigureAwait(false);
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : onFailure(result.Errors);
        }

        /// <summary>
        /// Asynchronously processes a <see cref="Task{TResult}"/> of <see cref="Result{TValue}"/> by applying the specified success or failure handling functions.
        /// </summary>
        /// <param name="onSuccess">A function to execute when the result is successful. The function receives the success value of type <typeparamref name="TValue"/> and returns a value of type <typeparamref name="TNext"/>.</param>
        /// <param name="onFailure">A function to execute when the result is a failure. The function receives a collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <typeparam name="TNext">The type of the value to be returned by the success or failure handling functions.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed success or failure handling function.</returns>
        public async Task<TNext> MatchAsync<TNext>(
            Func<TValue, TNext> onSuccess,
            Func<IEnumerable<Error>, Task<TNext>> onFailure)
        {
            var result = await task.ConfigureAwait(false);
            return result.IsSuccess
                ? onSuccess(result.Value)
                : await onFailure(result.Errors).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes a <see cref="Task{TResult}"/> of <see cref="Result{TValue}"/> asynchronously by executing the provided success or failure functions
        /// based on the state of the result.
        /// </summary>
        /// <param name="onSuccess">A function to execute when the result is successful. The function takes a success value of type <typeparamref name="TValue"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <param name="onFailure">A function to execute when the result is a failure. The function takes a collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
        /// <typeparam name="TNext">The type of the value to be returned by the functions.</typeparam>
        /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed function.</returns>
        public async Task<TNext> MatchAsync<TNext>(
            Func<TValue, Task<TNext>> onSuccess,
            Func<IEnumerable<Error>, Task<TNext>> onFailure)
        {
            var result = await task.ConfigureAwait(false);
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : await onFailure(result.Errors).ConfigureAwait(false);
        }
    }
}
