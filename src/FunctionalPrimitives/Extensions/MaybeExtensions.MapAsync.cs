namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybe)
    {
        /// <summary>
        /// Asynchronously transforms the value inside the awaited <see cref="Maybe{T}"/> when a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the mapped value.</typeparam>
        /// <param name="projection">The mapping function applied to the current value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Maybe{U}"/> containing the mapped value
        /// when the awaited <see cref="Maybe{T}"/> has a value; otherwise <see cref="Maybe.None"/>.
        /// </returns>
        public Task<Maybe<U>> MapAsync<U>(Func<T, U> projection)
        {
            return maybe.MatchAsync(
                x => Some(projection(x)),
                None<U>);
        }

        /// <summary>
        /// Projects the value inside the awaited <see cref="Maybe{T}"/> using LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="mapper">The projection function applied to the current value.</param>
        /// <returns>The same result as calling <c>MapAsync(mapper)</c>.</returns>
        public Task<Maybe<U>> Select<U>(Func<T, U> mapper)
            => maybe.MapAsync(mapper);
    }
}
