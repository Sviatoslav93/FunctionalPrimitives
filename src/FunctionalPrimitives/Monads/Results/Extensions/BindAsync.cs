namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Provides extension methods for working with the FunctionalPrimitives type, enabling
    /// operations such as asynchronous binding for transforming or handling
    /// results in a functional style.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result
    /// when the operation is successful.</typeparam>
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Asynchronously binds the result of a successful operation to an asynchronous function,
        /// enabling further transformations or operations. If the result represents a failure,
        /// the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that processes the successful value of the result
        /// and returns a task producing a new result.</param>
        /// <typeparam name="U">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the
        /// output result if the input result is successful; otherwise, the original errors are propagated.</returns>
        public async Task<Result<U>> BindAsync<U>(
            Func<T,
                Task<U>> onSuccess)
        {
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : Failure<U>(result.ErrorsInternal);
        }

        /// <summary>
        /// Asynchronously transforms the value of a successful result using the provided asynchronous function.
        /// If the result represents a failure, the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that takes the value of the result and returns a task
        /// producing a transformed result.</param>
        /// <typeparam name="U">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the transformed
        /// result if the input result is successful, otherwise the original errors are returned.</returns>
        public async Task<Result<U>> BindAsync<U>(
            Func<T, Task<Result<U>>> onSuccess)
        {
            return result.IsSuccess
                ? await onSuccess(result.Value).ConfigureAwait(false)
                : Failure<U>(result.ErrorsInternal);
        }

        /// <summary>
        /// Asynchronously transforms the value of a successful result using the provided asynchronous function.
        /// If the result represents a failure, the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that takes the value of the result and returns a task
        /// producing a transformed result.</param>
        /// <typeparam name="U">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the transformed
        /// result if the input result is successful, otherwise the original errors are returned.</returns>
        public async Task<Result<U>> SelectMany<U>(
            Func<T, Task<Result<U>>> onSuccess)
        {
            return await result.BindAsync(onSuccess).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously projects and flattens the result value using a binder and a projector.
        /// If any step fails, the corresponding errors are propagated.
        /// </summary>
        /// <param name="binder">A function that takes the source value and returns a task producing an intermediate result.</param>
        /// <param name="projector">A function that combines the source value and the intermediate value into the final value.</param>
        /// <typeparam name="V">The type of the intermediate successful value.</typeparam>
        /// <typeparam name="U">The type of the final successful value.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the projected result
        /// if all operations are successful; otherwise, propagated errors.</returns>
        public Task<Result<U>> SelectMany<V, U>(
            Func<T, Task<Result<V>>> binder,
            Func<T, V, U> projector)
        {
            return result.BindAsync(binder)
                .MapAsync(v => projector(result.Value, v));
        }
    }

    /// <summary>
    /// Provides extension methods for handling asynchronous operations on the FunctionalPrimitives type,
    /// such as binding transformations while retaining the FunctionalPrimitives semantics.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the FunctionalPrimitives when the operation is successful.</typeparam>
    extension<T>(Task<Result<T>> task)
    {
        /// <summary>
        /// Asynchronously binds the result of a `FunctionalPrimitives{T}` to an asynchronous function,
        /// enabling further transformations or operations. If the result represents a failure,
        /// the original errors are propagated without invoking the provided function.
        /// </summary>
        /// <param name="onSuccess">A function to process the successful value of the current result.
        /// The function returns a task representing either a transformed value or a transformed result.</param>
        /// <typeparam name="U">The type of the value in the resulting output after transformation.</typeparam>
        /// <returns>A task containing the resulting `FunctionalPrimitives{TNextValue}` after applying the provided function
        /// if the initial result is successful. Otherwise, it propagates the original errors.</returns>
        public async Task<Result<U>> BindAsync<U>(
            Func<T, U> onSuccess)
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
        /// <typeparam name="U">The type of the value in the result returned by the onSuccess function.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the
        /// transformed result if the input result is successful; otherwise, the original errors are propagated.</returns>
        public async Task<Result<U>> BindAsync<U>(
            Func<T, Result<U>> onSuccess)
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
        /// <typeparam name="U">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the
        /// output result if the input result is successful; otherwise, the original errors are propagated.</returns>
        public async Task<Result<U>> BindAsync<U>(
            Func<T, Task<U>> onSuccess)
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
        /// <typeparam name="U">The type of the value in the resulting output when the operation is successful.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the new result
        /// if the input result is successful; otherwise, it contains the original errors.</returns>
        public async Task<Result<U>> BindAsync<U>(
            Func<T, Task<Result<U>>> onSuccess)
        {
            var result = await task.ConfigureAwait(false);
            return await result.BindAsync(onSuccess);
        }

        /// <summary>
        /// Asynchronously transforms the value of a successful result using the provided asynchronous function.
        /// If the result represents a failure, the original errors are propagated.
        /// </summary>
        /// <param name="onSuccess">A function that takes the value of the result and returns a task
        /// producing a transformed result.</param>
        /// <typeparam name="U">The type of the successful value in the output result.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the transformed
        /// result if the input result is successful, otherwise the original errors are returned.</returns>
        public async Task<Result<U>> SelectMany<U>(
            Func<T, Task<Result<U>>> onSuccess)
        {
            var result = await task.ConfigureAwait(false);
            return await result.SelectMany(onSuccess).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously projects and flattens the result value using a binder and a projector.
        /// If any step fails, the corresponding errors are propagated.
        /// </summary>
        /// <param name="binder">A function that takes the source value and returns a task producing an intermediate result.</param>
        /// <param name="projector">A function that combines the source value and the intermediate value into the final value.</param>
        /// <typeparam name="V">The type of the intermediate successful value.</typeparam>
        /// <typeparam name="U">The type of the final successful value.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the projected result
        /// if all operations are successful; otherwise, propagated errors.</returns>
        public async Task<Result<U>> SelectMany<V, U>(
            Func<T, Task<Result<V>>> binder,
            Func<T, V, U> projector)
        {
            var result = await task.ConfigureAwait(false);
            return await result.SelectMany(binder, projector).ConfigureAwait(false);
        }
    }
}
