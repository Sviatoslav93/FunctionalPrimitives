namespace FunctionalPrimitives.Extensions.Maybe;

public static partial class MaybeExtensions
{
    extension<T>(Maybe<T> maybe)
    {
        /// <summary>
        /// Executes the specified action on the contained value if a value is present, then returns the original <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke with the contained value when <see cref="Maybe{T}.HasValue"/> is <see langword="true"/>.</param>
        /// <returns>The original <see cref="Maybe{T}"/>, unchanged.</returns>
        public Maybe<T> Tap(Action<T> action)
        {
            return maybe.Bind(value =>
            {
                action(value);
                return Some(value);
            });
        }

        /// <summary>
        /// Executes the specified action when no value is present, then returns the original <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke when <see cref="Maybe{T}.HasValue"/> is <see langword="false"/>.</param>
        /// <returns>The original <see cref="Maybe{T}"/>, unchanged.</returns>
        public Maybe<T> TapNone(Action action)
        {
            return maybe.Match(
                onSome: Some,
                onNone: () =>
                {
                    action();
                    return maybe;
                });
        }
    }
}
