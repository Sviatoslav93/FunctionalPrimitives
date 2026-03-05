namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Maybe<TValue> maybe)
    {
        public void Tap(Action<TValue> action)
        {
            if (maybe.HasValue)
            {
                action(maybe.Value);
            }
        }

        public void TapNone(Action action)
        {
            if (!maybe.HasValue)
            {
                action();
            }
        }
    }
}
