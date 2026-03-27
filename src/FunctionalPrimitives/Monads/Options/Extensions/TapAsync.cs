namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        /// <summary>
        /// Asynchronously executes the specified synchronous action on the contained value of the awaited
        /// <see cref="Option{T}"/> if a value is present, then returns the original <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke with the contained value when it is present.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the original <see cref="Option{T}"/>, unchanged.
        /// </returns>
        public Task<Option<T>> TapAsync(Action<T> action)
        {
            return optionTask.MatchAsync(
                value =>
                {
                    action(value);
                    return Some(value);
                },
                () => optionTask);
        }

        /// <summary>
        /// Executes the specified asynchronous action on the contained value of the awaited
        /// <see cref="Option{T}"/> if a value is present, then returns the original <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="action">The asynchronous action to invoke with the contained value when it is present.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the original <see cref="Option{T}"/>, unchanged.
        /// </returns>
        public Task<Option<T>> Tap(Func<T, Task> action)
        {
            return optionTask.MatchAsync(
                async value =>
                {
                    await action(value).ConfigureAwait(false);
                    return Some(value);
                },
                () => optionTask);
        }

        /// <summary>
        /// Asynchronously executes the specified action when the awaited <see cref="Option{T}"/> has no value,
        /// then returns the original <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke when no value is present.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the original <see cref="Option{T}"/>, unchanged.
        /// </returns>
        public Task<Option<T>> TapNoneAsync(Action action)
        {
            return optionTask.MatchAsync(
                _ => optionTask,
                () =>
                {
                    action();
                    return optionTask;
                });
        }

        /// <summary>
        /// Asynchronously executes the specified asynchronous action when the awaited <see cref="Option{T}"/> has no value,
        /// then returns the original <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="action">The asynchronous action to invoke when no value is present.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the original <see cref="Option{T}"/>, unchanged.
        /// </returns>
        public Task<Option<T>> TapNoneAsync(Func<Task> action)
        {
            return optionTask.MatchAsync(
                _ => optionTask,
                async () =>
                {
                    await action().ConfigureAwait(false);
                    return await optionTask;
                });
        }
    }
}
