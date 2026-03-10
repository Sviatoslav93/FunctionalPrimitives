namespace FunctionalPrimitives.Extensions.Maybe;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybe)
    {
        /// <summary>
        /// Asynchronously executes the specified synchronous action on the contained value of the awaited
        /// <see cref="Maybe{T}"/> if a value is present, then returns the original <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke with the contained value when it is present.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the original <see cref="Maybe{T}"/>, unchanged.
        /// </returns>
        public Task<Maybe<T>> TapAsync(Action<T> action)
        {
            return maybe.BindAsync(value =>
            {
                action(value);
                return Some(value);
            });
        }

        /// <summary>
        /// Executes the specified asynchronous action on the contained value of the awaited
        /// <see cref="Maybe{T}"/> if a value is present, then returns the original <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="action">The asynchronous action to invoke with the contained value when it is present.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the original <see cref="Maybe{T}"/>, unchanged.
        /// </returns>
        public Task<Maybe<T>> Tap(Func<T, Task> action)
        {
            return maybe.BindAsync(value =>
            {
                action(value);
                return Some(value);
            });
        }

        /// <summary>
        /// Asynchronously executes the specified action when the awaited <see cref="Maybe{T}"/> has no value,
        /// then returns the original <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke when no value is present.</param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to the original <see cref="Maybe{T}"/>, unchanged.
        /// </returns>
        public Task TapNoneAsync(Action action)
        {
            return maybe.MatchAsync(
                onSome: x => Task.FromResult(Some(x)),
                onNone: () =>
                {
                    action();
                    return maybe;
                });
        }
    }
}
