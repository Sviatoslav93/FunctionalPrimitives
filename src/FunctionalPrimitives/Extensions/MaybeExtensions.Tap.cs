namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<T>(Maybe<T> maybe)
    {
        public Maybe<T> Tap(Action<T> action)
        {
            return maybe.Bind(value =>
            {
                action(value);
                return Some(value);
            });
        }

        public Maybe<T> TapNone(Action action)
        {
            return maybe.Match(
                onSome: Some,
                onNone: () =>
                {
                    action();
                    return maybe;
                });
        }
    }
}
