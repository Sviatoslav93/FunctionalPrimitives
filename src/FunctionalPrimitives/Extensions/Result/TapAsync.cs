namespace FunctionalPrimitives.Extensions.Result;

public partial class ResultExtensions
{
    /// <summary>
    /// Contains extension methods for performing asynchronous operations on results.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result object to operate on.</param>
    /// <returns>An extended result object supporting asynchronous operations.</returns>
    extension<T>(Result<T> result)
    {
        /// <summary>
        /// Executes an asynchronous action on the value of a successful result.
        /// </summary>
        /// <param name="action">The asynchronous action to execute on the value of the result.</param>
        /// <returns>A task representing the result, retaining its original state unless the action modifies it.</returns>
        public Task<Result<T>> TapAsync(
            Func<T, Task> action)
        {
            return result.BindAsync(async x =>
            {
                await action(x).ConfigureAwait(false);

                return x;
            });
        }

        /// <summary>
        /// Executes an asynchronous action on the errors of a failed result.
        /// </summary>
        /// <param name="action">The asynchronous action to execute on the errors of the result.</param>
        /// <returns>A task representing the result, retaining its original state regardless of the action execution.</returns>
        public async Task<Result<T>> TapErrorAsync(
            Func<IEnumerable<Error>, Task> action)
        {
            if (!result.IsSuccess)
            {
                await action(result.Errors).ConfigureAwait(false);
            }

            return result;
        }
    }

    /// <param name="task">A task representing the asynchronous result operation.</param>
    /// <typeparam name="TValue">The type of the value contained in the result.</typeparam>
    extension<TValue>(Task<Result<TValue>> task)
    {
        /// <summary>
        /// Executes a synchronous action on the value of a successful result when the result is represented as a task.
        /// </summary>
        /// <param name="action">The synchronous action to execute on the value of the result if it is successful.</param>
        /// <returns>A task containing the result of the operation, retaining its original state unless the action modifies it.</returns>
        public async Task<Result<TValue>> TapAsync(Action<TValue> action)
        {
            var result = await task.ConfigureAwait(false);
            return result.Tap(action);
        }

        /// <summary>
        /// Executes an asynchronous action on the value of a successful result contained within a task.
        /// </summary>
        /// <param name="action">The asynchronous action to execute on the value of the successful result.</param>
        /// <returns>A task that represents the result, retaining its original state unless the action modifies it.</returns>
        public async Task<Result<TValue>> TapAsync(Func<TValue, Task> action)
        {
            var result = await task.ConfigureAwait(false);
            return await result.TapAsync(action).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes an asynchronous action on the errors of a failed result, if any.
        /// </summary>
        /// <param name="action">The asynchronous action to execute on the errors of the result.</param>
        /// <returns>A task representing the result, retaining its original state unless the action modifies it.</returns>
        public async Task<Result<TValue>> TapErrorAsync(Func<IEnumerable<Error>, Task> action)
        {
            var result = await task.ConfigureAwait(false);
            return await result.TapErrorAsync(action).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes an asynchronous action on the errors of a failed result, if any.
        /// </summary>
        /// <param name="action">The asynchronous action to execute on the errors of the result.</param>
        /// <returns>A task representing the result, retaining its original state unless the action modifies it.</returns>
        public async Task<Result<TValue>> TapErrorAsync(Action<IEnumerable<Error>> action)
        {
            var result = await task.ConfigureAwait(false);
            action(result.Errors);

            return result;
        }
    }
}
