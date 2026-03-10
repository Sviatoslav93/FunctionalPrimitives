namespace FunctionalPrimitives.Extensions.Maybe;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybe)
    {
        /// <summary>
        /// Asynchronously matches the state of the awaited <see cref="Maybe{T}"/> and executes
        /// the appropriate function based on whether a value is present.
        /// </summary>
        /// <typeparam name="U">The type of the result returned by the provided functions.</typeparam>
        /// <param name="onSome">A function to execute if a value is present. Takes the value as an argument and returns a result of type <typeparamref name="U"/>.</param>
        /// <param name="onNone">A function to execute if no value is present. Returns a result of type <typeparamref name="U"/>.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that resolves to the result of invoking either <paramref name="onSome"/>
        /// or <paramref name="onNone"/>, depending on the state of the awaited <see cref="Maybe{T}"/>.
        /// </returns>
        public async Task<U> MatchAsync<U>(
            Func<T, U> onSome,
            Func<U> onNone)
        {
            return (await maybe.ConfigureAwait(false)).Match(onSome, onNone);
        }
    }
}
