namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Maybe<TValue> maybe)
    {
        public Maybe<TNext> Bind<TNext>(Func<TValue, Maybe<TNext>> binder)
        {
            return maybe.HasValue
                ? binder(maybe.Value)
                : Maybe<TNext>.None;
        }

        public Maybe<TNext> SelectMany<TNext>(Func<TValue, Maybe<TNext>> binder)
        {
            return maybe.Bind(binder);
        }
    }
}
