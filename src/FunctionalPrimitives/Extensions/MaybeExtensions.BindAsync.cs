namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybeTask)
    {
        public async Task<Maybe<U>> BindAsync<U>(Func<T, Maybe<U>> binder)
        {
            var maybe = await maybeTask.ConfigureAwait(false);
            return maybe.Bind(binder);
        }

        public Task<Maybe<U>> SelectMany<U>(Func<T, Maybe<U>> binder)
            => maybeTask.BindAsync(binder);

        public Task<Maybe<U>> SelectMany<V, U>(
            Func<T, Maybe<V>> binder,
            Func<T, V, U> projector)
            => maybeTask.BindAsync(t =>
                binder(t).Map(i => projector(t, i)));
    }
}
