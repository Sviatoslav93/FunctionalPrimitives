namespace FunctionalPrimitives.Extensions.Maybe;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybeTask)
    {
        /// <summary>
        /// Asynchronously applies a binder function to the value inside the awaited <see cref="Maybe{T}"/> if a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Maybe{U}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Maybe{U}"/>.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Maybe{U}"/> containing the result of the binder function
        /// if a value is present; otherwise, a <see cref="Maybe{U}"/> with no value.
        /// </returns>
        public async Task<Maybe<U>> BindAsync<U>(Func<T, Maybe<U>> binder)
        {
            var maybe = await maybeTask.ConfigureAwait(false);
            return maybe.Bind(binder);
        }

        /// <summary>
        /// Projects the value inside the awaited <see cref="Maybe{T}"/> to another <see cref="Maybe{U}"/>
        /// using the specified binder function, enabling LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Maybe{U}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Maybe{U}"/>.</param>
        /// <returns>The same result as calling <c>BindAsync(binder)</c>.</returns>
        public Task<Maybe<U>> SelectMany<U>(Func<T, Maybe<U>> binder)
            => maybeTask.BindAsync(binder);

        /// <summary>
        /// Projects each element of the awaited <see cref="Maybe{T}"/> into a new form using a binder
        /// and applies a result selector to produce the final result, enabling LINQ query syntax.
        /// </summary>
        /// <typeparam name="V">The type of the intermediate value returned by the binder function.</typeparam>
        /// <typeparam name="U">The type of the final result produced by the selector function.</typeparam>
        /// <param name="binder">A function applied to the current value that returns a <see cref="Maybe{V}"/>.</param>
        /// <param name="projector">A function that combines the original and intermediate values into the final value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Maybe{U}"/> containing the projected result
        /// if a value is present at every step; otherwise, a <see cref="Maybe{U}"/> with no value.
        /// </returns>
        public Task<Maybe<U>> SelectMany<V, U>(
            Func<T, Maybe<V>> binder,
            Func<T, V, U> projector)
            => maybeTask.BindAsync(t =>
                binder(t).Map(i => projector(t, i)));
    }
}
