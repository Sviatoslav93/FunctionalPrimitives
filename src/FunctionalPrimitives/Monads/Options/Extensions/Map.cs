namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Transforms the value inside the <see cref="Option{T}"/> when a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the mapped value.</typeparam>
        /// <param name="projection">The mapping function applied to the current value.</param>
        /// <returns>
        /// A <see cref="Option{T}"/> containing the mapped value when <paramref name="option"/> has a value;
        /// otherwise <see cref="None"/>.
        /// </returns>
        public Option<U> Map<U>(Func<T, U> projection)
        {
            return option.Match(
                x => Some(projection(x)),
                None<U>);
        }

        /// <summary>
        /// Projects the value inside the <see cref="Option{T}"/> using LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="projection">The projection function applied to the current value.</param>
        /// <returns>The same result as calling <c>Map(mapper)</c>.</returns>
        public Option<U> Select<U>(Func<T, U> projection)
        {
            return option.Map(projection);
        }
    }
}
