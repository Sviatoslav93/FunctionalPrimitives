namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Task<Maybe<TValue>> task)
    {
        public async Task TapAsync(Action<TValue> action)
        {
            var maybe = await task.ConfigureAwait(false);
            maybe.Tap(action);
        }

        public async Task TapNoneAsync(Action action)
        {
            var maybe = await task.ConfigureAwait(false);
            maybe.TapNone(action);
        }
    }
}
