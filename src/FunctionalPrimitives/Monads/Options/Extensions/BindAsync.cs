namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        /// <summary>
        /// Asynchronously applies a binder function to the value inside the awaited <see cref="Option{T}"/> if a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Option{T}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Option{T}"/>.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Option{T}"/> containing the result of the binder function
        /// if a value is present; otherwise, a <see cref="Option{T}"/> with no value.
        /// </returns>
        public async Task<Option<U>> BindAsync<U>(Func<T, Option<U>> binder)
        {
            var maybe = await optionTask.ConfigureAwait(false);
            return maybe.Bind(binder);
        }

        /// <summary>
        /// Asynchronously applies an asynchronous binder function to the value inside the awaited <see cref="Option{T}"/> if a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Option{T}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Option{T}"/> asynchronously.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Option{T}"/> containing the result of the binder function
        /// if a value is present; otherwise, a <see cref="Option{T}"/> with no value.
        /// </returns>
        public async Task<Option<U>> BindAsync<U>(Func<T, Task<Option<U>>> binder)
        {
            var maybe = await optionTask.ConfigureAwait(false);

            return maybe.HasValue
                ? await binder(maybe.Value).ConfigureAwait(false)
                : None<U>();
        }

        /// <summary>
        /// Projects the value inside the awaited <see cref="Option{T}"/> to another <see cref="Option{T}"/>
        /// using the specified binder function, enabling LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the value in the resulting <see cref="Option{T}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Option{T}"/>.</param>
        /// <returns>The same result as calling <c>BindAsync(binder)</c>.</returns>
        public Task<Option<U>> SelectMany<U>(Func<T, Option<U>> binder)
            => optionTask.BindAsync(binder);

        /// <summary>
        /// Projects each element of the awaited <see cref="Option{T}"/> into a new form using a binder
        /// and applies a result selector to produce the final result, enabling LINQ query syntax.
        /// </summary>
        /// <typeparam name="V">The type of the intermediate value returned by the binder function.</typeparam>
        /// <typeparam name="U">The type of the final result produced by the selector function.</typeparam>
        /// <param name="binder">A function applied to the current value that returns a <see cref="Option{T}"/>.</param>
        /// <param name="projector">A function that combines the original and intermediate values into the final value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Option{T}"/> containing the projected result
        /// if a value is present at every step; otherwise, a <see cref="Option{T}"/> with no value.
        /// </returns>
        public Task<Option<U>> SelectMany<V, U>(
            Func<T, Option<V>> binder,
            Func<T, V, U> projector)
            => optionTask.BindAsync(t =>
                binder(t).Map(i => projector(t, i)));

        /// <summary>
        /// Projects each element of the awaited <see cref="Option{T}"/> into a new form using an asynchronous binder
        /// and applies a result selector to produce the final result, enabling LINQ query syntax.
        /// </summary>
        /// <typeparam name="V">The type of the intermediate value returned by the binder function.</typeparam>
        /// <typeparam name="U">The type of the final result produced by the selector function.</typeparam>
        /// <param name="binder">A function applied to the current value that returns a <see cref="Option{T}"/> asynchronously.</param>
        /// <param name="projector">A function that combines the original and intermediate values into the final value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to a <see cref="Option{T}"/> containing the projected result
        /// if a value is present at every step; otherwise, a <see cref="Option{T}"/> with no value.
        /// </returns>
        public Task<Option<U>> SelectMany<V, U>(
            Func<T, Task<Option<V>>> binder,
            Func<T, V, U> projector)
            => optionTask.BindAsync(t =>
                binder(t).MapAsync(i => projector(t, i)));
    }
}
