namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybe)
    {
        public async Task<TNext> MatchAsync<TNext>(
            Func<T, TNext> onSome,
            Func<TNext> onNone)
        {
            return (await maybe.ConfigureAwait(false)).Match(onSome, onNone);
        }
    }
}
