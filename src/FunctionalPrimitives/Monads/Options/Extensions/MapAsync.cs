namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        /// <summary>
        /// Asynchronously transforms the value inside the awaited <see cref="Option{T}"/> when a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the mapped value.</typeparam>
        /// <param name="projection">The mapping function applied to the current value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Option{T}"/> containing the mapped value
        /// when the awaited <see cref="Option{T}"/> has a value; otherwise <see cref="None"/>.
        /// </returns>
        public Task<Option<U>> MapAsync<U>(Func<T, U> projection)
        {
            return optionTask.MatchAsync(
                x => Some(projection(x)),
                None<U>);
        }

        /// <summary>
        /// Asynchronously transforms the value inside the awaited <see cref="Option{T}"/> using an asynchronous projection function when a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the mapped value.</typeparam>
        /// <param name="projection">The asynchronous mapping function applied to the current value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Option{T}"/> containing the mapped value
        /// when the awaited <see cref="Option{T}"/> has a value; otherwise <see cref="None"/>.
        /// </returns>
        public Task<Option<U>> MapAsync<U>(Func<T, Task<U>> projection)
        {
            return optionTask.MatchAsync(
                async x => Some(await projection(x).ConfigureAwait(false)),
                None<U>);
        }

        /// <summary>
        /// Projects the value inside the awaited <see cref="Option{T}"/> using LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="mapper">The projection function applied to the current value.</param>
        /// <returns>The same result as calling <c>MapAsync(mapper)</c>.</returns>
        public Task<Option<U>> Select<U>(Func<T, U> mapper)
            => optionTask.MapAsync(mapper);

        /// <summary>
        /// Projects the value inside the awaited <see cref="Option{T}"/> using an asynchronous projection function and LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="mapper">The asynchronous projection function applied to the current value.</param>
        /// <returns>The same result as calling <c>MapAsync(mapper)</c>.</returns>
        public Task<Option<U>> Select<U>(Func<T, Task<U>> mapper)
            => optionTask.MapAsync(mapper);
    }
}
