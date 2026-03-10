namespace FunctionalPrimitives.Extensions.Maybe;

public static partial class MaybeExtensions
{
    extension<T>(Maybe<T> maybe)
    {
        /// <summary>
        /// Transforms the value inside the <see cref="Maybe{T}"/> when a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the mapped value.</typeparam>
        /// <param name="projection">The mapping function applied to the current value.</param>
        /// <returns>
        /// A <see cref="Maybe{U}"/> containing the mapped value when <paramref name="maybe"/> has a value;
        /// otherwise <see cref="None"/>.
        /// </returns>
        public Maybe<U> Map<U>(Func<T, U> projection)
        {
            return maybe.Match(
                x => Some(projection(x)),
                None<U>);
        }

        /// <summary>
        /// Projects the value inside the <see cref="Maybe{T}"/> using LINQ query syntax.
        /// </summary>
        /// <typeparam name="U">The type of the projected value.</typeparam>
        /// <param name="projection">The projection function applied to the current value.</param>
        /// <returns>The same result as calling <c>Map(mapper)</c>.</returns>
        public Maybe<U> Select<U>(Func<T, U> projection)
        {
            return maybe.Map(projection);
        }
    }
}
