namespace FunctionalPrimitives.Monads.Options.Extensions;

/// <summary>
/// Extension methods for <see cref="Option{T}"/>.
/// </summary>
public static partial class MaybeExtensions
{
    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Matches the current state of the <see cref="Option{T}"/> and executes the appropriate function based on whether a value is available.
        /// </summary>
        /// <typeparam name="U">The type of the result returned by the provided functions.</typeparam>
        /// <param name="onSome">A function to execute if a value is present. The function takes the value as an argument and returns a result of type <typeparamref name="U"/>.</param>
        /// <param name="onNone">A function to execute if no value is present. The function returns a result of type <typeparamref name="U"/>.</param>
        /// <returns>The result of type <typeparamref name="U"/> obtained by invoking either <paramref name="onSome"/> or <paramref name="onNone"/>, depending on the state of the given <paramref name="option"/>.</returns>
        public U Match<U>(
            Func<T, U> onSome,
            Func<U> onNone)
        {
            return option.HasValue
                ? onSome(option.Value)
                : onNone();
        }
    }
}
