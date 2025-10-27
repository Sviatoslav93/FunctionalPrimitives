namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Matches the result asynchronously by executing the provided success or failure functions
    /// based on the state of the result.
    /// </summary>
    /// <param name="result">The result to be processed. Can be in a success or failure state.</param>
    /// <param name="onSuccess">A function to execute when the result is successful. The function takes the success value of type <typeparamref name="TValue"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <param name="onFailure">A function to execute when the result is a failure. The function takes the collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated in the result when successful.</typeparam>
    /// <typeparam name="TNext">The type of the value to be returned by the functions.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed function.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Result<TValue> result,
        Func<TValue, Task<TNext>> onSuccess,
        Func<IEnumerable<Error>, Task<TNext>> onFailure)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// Processes the result asynchronously by executing the provided success or failure functions
    /// based on the state of the result.
    /// </summary>
    /// <param name="result">The result to be processed. Indicates success or failure along with associated data.</param>
    /// <param name="onSuccess">A function invoked when the result is successful. This function receives the success value of type <typeparamref name="TValue"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <param name="onFailure">A function invoked when the result is a failure. This function receives the collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated in the result when successful.</typeparam>
    /// <typeparam name="TNext">The type of the value to be returned by the executed functions.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> representing the output of either the success or failure function, depending on the result's state.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Result<TValue> result,
        Func<TValue, Task<TNext>> onSuccess,
        Func<IEnumerable<Error>, TNext> onFailure)
    {
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : onFailure(result.Errors);
    }

    /// <summary>
    /// Processes a <see cref="Result{TValue}"/> by executing the provided success or failure functions
    /// based on its state, and returns a value of type <typeparamref name="TNext"/>.
    /// </summary>
    /// <param name="result">The result to process, which can either be in a success or failure state.</param>
    /// <param name="onSuccess">A function to execute if the result is successful. It takes the success value of type <typeparamref name="TValue"/> and returns a value of type <typeparamref name="TNext"/>.</param>
    /// <param name="onFailure">A function to execute if the result is in a failure state. It takes a collection of <see cref="Error"/> and returns a value of type <typeparamref name="TNext"/> asynchronously as a <see cref="Task{TNext}"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated within the result when it is successful.</typeparam>
    /// <typeparam name="TNext">The type of the value that the provided functions return.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> representing the result of the invoked success or failure function.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Result<TValue> result,
        Func<TValue, TNext> onSuccess,
        Func<IEnumerable<Error>, Task<TNext>> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }

    /// <summary>
    /// Processes a task containing a result either by invoking the success function with the value from the result
    /// or the failure function with the collection of errors, and returns the computed value.
    /// </summary>
    /// <param name="task">A task containing the result to be processed. The result can either be successful or a failure.</param>
    /// <param name="onSuccess">A function to execute when the result is successful. Takes the success value of type <typeparamref name="TValue"/> and returns a value of type <typeparamref name="TNext"/>.</param>
    /// <param name="onFailure">A function to execute when the result is a failure. Takes a collection of <see cref="Error"/> and returns a value of type <typeparamref name="TNext"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated in the successful result.</typeparam>
    /// <typeparam name="TNext">The type of the value to be returned by the success or failure functions.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> that represents the result of applying either the success or failure function.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, TNext> onSuccess,
        Func<IEnumerable<Error>, TNext> onFailure)
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
    /// <param name="task">The task containing the result to be processed. The result can be in a success or failure state.</param>
    /// <param name="onSuccess">A function to execute when the result is successful. The function takes the success value of type <typeparamref name="TValue"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <param name="onFailure">A function to execute when the result is a failure. The function takes the collection of <see cref="Error"/> and returns a value of type <typeparamref name="TNext"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated in the result when successful.</typeparam>
    /// <typeparam name="TNext">The type of the value to be returned by the functions.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed function.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
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
    /// <param name="task">The task representing the result to be processed. The result can either be successful or failed.</param>
    /// <param name="onSuccess">A function to execute when the result is successful. The function receives the success value of type <typeparamref name="TValue"/> and returns a value of type <typeparamref name="TNext"/>.</param>
    /// <param name="onFailure">A function to execute when the result is a failure. The function receives a collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated by the result, when successful.</typeparam>
    /// <typeparam name="TNext">The type of the value to be returned by the success or failure handling functions.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed success or failure handling function.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
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
    /// <param name="task">The task encapsulating the result to be processed. Can be in a success or failure state.</param>
    /// <param name="onSuccess">A function to execute when the result is successful. The function takes a success value of type <typeparamref name="TValue"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <param name="onFailure">A function to execute when the result is a failure. The function takes a collection of <see cref="Error"/> and returns a <see cref="Task{TNext}"/>.</param>
    /// <typeparam name="TValue">The type of the value encapsulated in the result when successful.</typeparam>
    /// <typeparam name="TNext">The type of the value to be returned by the functions.</typeparam>
    /// <returns>A <see cref="Task{TNext}"/> representing the result of the executed function.</returns>
    public static async Task<TNext> MatchAsync<TValue, TNext>(
        this Task<Result<TValue>> task,
        Func<TValue, Task<TNext>> onSuccess,
        Func<IEnumerable<Error>, Task<TNext>> onFailure)
    {
        var result = await task.ConfigureAwait(false);
        return result.IsSuccess
            ? await onSuccess(result.Value).ConfigureAwait(false)
            : await onFailure(result.Errors).ConfigureAwait(false);
    }
}
