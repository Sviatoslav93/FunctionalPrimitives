namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Provides extension methods for working with the Result type, enabling
    /// operations such as asynchronous binding for transforming or handling
    /// results in a functional style.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the result
    /// when the operation is successful.</typeparam>
    extension<TValue>(Result<TValue> result)
    {
        /// <summary>
        /// Asynchronously binds the result of a successful operation to an asynchronous function,
        /// enabling further transformations or operations. If the result represents a failure,
        /// the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that processes the successful value of the result
        /// and returns a task producing a new result.</param>
        /// <typeparam name="TNextValue">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the
        /// output result if the input result is successful; otherwise, the original errors are propagated.</returns>
        public async Task<Result<TNextValue>> BindAsync<TNextValue>(
            Func<TValue,
                Task<TNextValue>> onSuccess)
        {
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : result.Errors.ToArray();
        }

        /// <summary>
        /// Asynchronously transforms the value of a successful result using the provided asynchronous function.
        /// If the result represents a failure, the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that takes the value of the result and returns a task
        /// producing a transformed result.</param>
        /// <typeparam name="TNextValue">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the transformed
        /// result if the input result is successful, otherwise the original errors are returned.</returns>
        public async Task<Result<TNextValue>> BindAsync<TNextValue>(
            Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : result.Errors.ToArray();
        }
    }

    /// <summary>
    /// Provides extension methods for handling asynchronous operations on the Result type,
    /// such as binding transformations while retaining the Result semantics.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the Result when the operation is successful.</typeparam>
    extension<TValue>(Task<Result<TValue>> task)
    {
        /// <summary>
        /// Asynchronously binds the result of a `Result{TValue}` to an asynchronous function,
        /// enabling further transformations or operations. If the result represents a failure,
        /// the original errors are propagated without invoking the provided function.
        /// </summary>
        /// <param name="onSuccess">A function to process the successful value of the current result.
        /// The function returns a task representing either a transformed value or a transformed result.</param>
        /// <typeparam name="TNextValue">The type of the value in the resulting output after transformation.</typeparam>
        /// <returns>A task containing the resulting `Result{TNextValue}` after applying the provided function
        /// if the initial result is successful. Otherwise, it propagates the original errors.</returns>
        public async Task<Result<TNextValue>> BindAsync<TNextValue>(
            Func<TValue, TNextValue> onSuccess)
        {
            var result = await task.ConfigureAwait(false);
            return result.Bind(onSuccess);
        }

        /// <summary>
        /// Asynchronously binds a successful result to a function that produces a new result,
        /// enabling further transformations or operations. If the input result represents a failure,
        /// the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that processes the successful value of the result
        /// and returns an asynchronous operation producing a new result.</param>
        /// <typeparam name="TNextValue">The type of the value in the result returned by the onSuccess function.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the
        /// transformed result if the input result is successful; otherwise, the original errors are propagated.</returns>
        public async Task<Result<TNextValue>> BindAsync<TNextValue>(
            Func<TValue, Result<TNextValue>> onSuccess)
        {
            var result = await task.ConfigureAwait(false);
            return result.Bind(onSuccess);
        }

        /// <summary>
        /// Asynchronously binds the result of a successful operation to an asynchronous function,
        /// enabling further transformations or operations. If the result represents a failure,
        /// the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that processes the successful value of the result
        /// and returns a task producing a new value or transformation.</param>
        /// <typeparam name="TNextValue">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the
        /// output result if the input result is successful; otherwise, the original errors are propagated.</returns>
        public async Task<Result<TNextValue>> BindAsync<TNextValue>(
            Func<TValue, Task<TNextValue>> onSuccess)
        {
            var result = await task.ConfigureAwait(false);
            return await result.BindAsync(onSuccess);
        }

        /// <summary>
        /// Asynchronously binds the result of a successful operation to a given asynchronous function,
        /// facilitating further transformations or operations. If the result signifies failure,
        /// the original errors are returned without invoking the function.
        /// </summary>
        /// <param name="onSuccess">A function to process the successful value of the result,
        /// returning a task that yields a new result.</param>
        /// <typeparam name="TNextValue">The type of the value in the resulting output when the operation is successful.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the new result
        /// if the input result is successful; otherwise, it contains the original errors.</returns>
        public async Task<Result<TNextValue>> BindAsync<TNextValue>(
            Func<TValue, Task<Result<TNextValue>>> onSuccess)
        {
            var result = await task.ConfigureAwait(false);
            return await result.BindAsync(onSuccess);
        }
    }
}
