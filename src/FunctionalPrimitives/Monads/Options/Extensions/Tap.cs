namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Executes the specified action on the contained value if a value is present, then returns the original <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke with the contained value when <see cref="Option{T}.HasValue"/> is <see langword="true"/>.</param>
        /// <returns>The original <see cref="Option{T}"/>, unchanged.</returns>
        public Option<T> Tap(Action<T> action)
        {
            return option.Bind(value =>
            {
                action(value);
                return Some(value);
            });
        }

        /// <summary>
        /// Executes the specified action when no value is present, then returns the original <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="action">The action to invoke when <see cref="Option{T}.HasValue"/> is <see langword="false"/>.</param>
        /// <returns>The original <see cref="Option{T}"/>, unchanged.</returns>
        public Option<T> TapNone(Action action)
        {
            return option.Match(
                onSome: Some,
                onNone: () =>
                {
                    action();
                    return option;
                });
        }
    }
}
