namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybe)
    {
        public Task<Maybe<U>> MapAsync<U>(Func<T, U> projection)
        {
            return maybe.MatchAsync(
                x => Some(projection(x)),
                None<U>);
        }

        public Task<Maybe<U>> Select<U>(Func<T, U> mapper)
            => maybe.MapAsync(mapper);
    }
}
