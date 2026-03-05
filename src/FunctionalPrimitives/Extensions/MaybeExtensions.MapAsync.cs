namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Task<Maybe<TValue>> task)
    {
        public async Task<Maybe<TNext>> Map<TNext>(Func<TValue, TNext> mapper)
        {
            var maybe = await task.ConfigureAwait(false);
            return maybe.Map(mapper);
        }

        public Task<Maybe<TResult>> Select<TResult>(Func<TValue, TResult> mapper)
            => task.Map(mapper);
    }
}
