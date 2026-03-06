namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Task<Maybe<T>> maybe)
    {
        public Task<Maybe<T>> TapAsync(Action<T> action)
        {
            return maybe.BindAsync(value =>
            {
                action(value);
                return Some(value);
            });
        }

        public Task<Maybe<T>> Tap(Func<T, Task> action)
        {
            return maybe.BindAsync(value =>
            {
                action(value);
                return Some(value);
            });
        }

        public Task TapNoneAsync(Action action)
        {
            return maybe.MatchAsync(
                onSome: x => Task.FromResult(Some(x)),
                onNone: () =>
                {
                    action();
                    return maybe;
                });
        }
    }
}
