namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Task<Maybe<TValue>> task)
    {
        /// <summary>
        /// Asynchronously matches the current state of the <see cref="Maybe{TValue}"/> and executes the appropriate function based on whether a value is available.
        /// </summary>
        /// <typeparam name="TNext">The type of the result returned by the provided functions.</typeparam>
        /// <param name="onSome">A function to execute if a value is present. The function takes the value as an argument and returns a result of type <typeparamref name="TNext"/>.</param>
        /// <param name="onNone">A function to execute if no value is present. The function returns a result of type <typeparamref name="TNext"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of type <typeparamref name="TNext"/> obtained by invoking either <paramref name="onSome"/> or <paramref name="onNone"/>, depending on the state of the given <paramref name="task"/>.</returns>
        public async Task<TNext> MatchAsync<TNext>(
            Func<TValue, TNext> onSome,
            Func<TNext> onNone)
        {
            var maybe = await task.ConfigureAwait(false);
            return maybe.Match(onSome, onNone);
        }
    }
}
