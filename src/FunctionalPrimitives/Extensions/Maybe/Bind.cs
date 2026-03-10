namespace FunctionalPrimitives.Extensions.Maybe;

public static partial class MaybeExtensions
{
    extension<T>(Maybe<T> maybe)
    {
        /// <summary>
        /// Applies a function to the value contained in the <see cref="Maybe{T}"/> if a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the value returned by the binder function.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Maybe{U}"/>.</param>
        /// <returns>
        /// A <see cref="Maybe{U}"/> containing the result of the binder function if a value is present;
        /// otherwise, a <see cref="Unit"/>.
        /// </returns>
        public Maybe<U> Bind<U>(Func<T, Maybe<U>> binder)
        {
            return maybe.HasValue
                ? binder(maybe.Value)
                : None<U>();
        }

        /// <summary>
        /// Projects the value contained in the <see cref="Maybe{T}"/> to another <see cref="Maybe{U}"/> using a specified transformation function.
        /// </summary>
        /// <typeparam name="U">The type of the value contained in the resulting <see cref="Maybe{U}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Maybe{U}"/>.</param>
        /// <returns>
        /// A <see cref="Maybe{U}"/> containing the result of the transformation if a value is present;
        /// otherwise, a <see cref="Unit"/>.
        /// </returns>
        public Maybe<U> SelectMany<U>(Func<T, Maybe<U>> binder)
        {
            return maybe.Bind(binder);
        }

        /// <summary>
        /// Projects each element of the <see cref="Maybe{T}"/> into a new form using a binding function
        /// and applies a result selector function to produce a final result.
        /// </summary>
        /// <typeparam name="V">The type of the intermediate value obtained from the binding function.</typeparam>
        /// <typeparam name="U">The type of the final result produced by the selector function.</typeparam>
        /// <param name="binder">A function to apply to the value contained in the <see cref="Maybe{T}"/>.
        /// This function returns a <see cref="Maybe{TIntermediate}"/>.</param>
        /// <param name="projector">A function to produce the result value given the initial value and the intermediate value.</param>
        /// <returns>
        /// A <see cref="Maybe{TFinal}"/> containing the final result of the projection if a value is present;
        /// otherwise, a <see cref="Unit"/>.
        /// </returns>
        public Maybe<U> SelectMany<V, U>(
            Func<T, Maybe<V>> binder,
            Func<T, V, U> projector)
        {
            return maybe.Bind(t =>
                binder(t).Map(i => projector(t, i)));
        }
    }
}

