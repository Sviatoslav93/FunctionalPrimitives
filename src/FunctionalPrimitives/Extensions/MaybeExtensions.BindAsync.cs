namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Task<Maybe<TValue>> task)
    {
        public async Task<Maybe<TNext>> Bind<TNext>(Func<TValue, Maybe<TNext>> binder)
        {
            var maybe = await task.ConfigureAwait(false);
            return maybe.Bind(binder);
        }

        public Task<Maybe<TNext>> SelectMany<TNext>(Func<TValue, Maybe<TNext>> binder)
            => task.Bind(binder);

        public Task<Maybe<TFinal>> SelectMany<TIntermediate, TFinal>(
            Func<TValue, Maybe<TIntermediate>> binder,
            Func<TValue, TIntermediate, TFinal> projector)
            => task.Bind(t =>
                binder(t).Map(i => projector(t, i)));
    }
}
