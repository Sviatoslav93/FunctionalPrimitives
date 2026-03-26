namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Applies a function to the value contained in the <see cref="Option{T}"/> if a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the value returned by the binder function.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Option{T}"/>.</param>
        /// <returns>
        /// A <see cref="Option{T}"/> containing the result of the binder function if a value is present;
        /// otherwise, a <see cref="Unit"/>.
        /// </returns>
        public Option<U> Bind<U>(Func<T, Option<U>> binder)
        {
            return option.HasValue
                ? binder(option.Value)
                : None<U>();
        }

        /// <summary>
        /// Projects the value contained in the <see cref="Option{T}"/> to another <see cref="Option{T}"/> using a specified transformation function.
        /// </summary>
        /// <typeparam name="U">The type of the value contained in the resulting <see cref="Option{T}"/>.</typeparam>
        /// <param name="binder">A function that takes the current value and returns a new <see cref="Option{T}"/>.</param>
        /// <returns>
        /// A <see cref="Option{T}"/> containing the result of the transformation if a value is present;
        /// otherwise, a <see cref="Unit"/>.
        /// </returns>
        public Option<U> SelectMany<U>(Func<T, Option<U>> binder)
        {
            return option.Bind(binder);
        }

        /// <summary>
        /// Projects each element of the <see cref="Option{T}"/> into a new form using a binding function
        /// and applies a result selector function to produce a final result.
        /// </summary>
        /// <typeparam name="V">The type of the intermediate value obtained from the binding function.</typeparam>
        /// <typeparam name="U">The type of the final result produced by the selector function.</typeparam>
        /// <param name="binder">A function to apply to the value contained in the <see cref="Option{T}"/>.
        /// This function returns a <see cref="Option{T}"/>.</param>
        /// <param name="projector">A function to produce the result value given the initial value and the intermediate value.</param>
        /// <returns>
        /// A <see cref="Option{T}"/> containing the final result of the projection if a value is present;
        /// otherwise, a <see cref="Unit"/>.
        /// </returns>
        public Option<U> SelectMany<V, U>(
            Func<T, Option<V>> binder,
            Func<T, V, U> projector)
        {
            return option.Bind(t =>
                binder(t).Map(i => projector(t, i)));
        }
    }
}

