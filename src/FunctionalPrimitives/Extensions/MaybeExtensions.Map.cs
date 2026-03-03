namespace FunctionalPrimitives.Extensions;

public static partial class MaybeExtensions
{
    extension<TValue>(Maybe<TValue> maybe)
    {
        public Maybe<TNext> Map<TNext>(Func<TValue, TNext> mapper)
        {
            return maybe.HasValue
                ? Maybe<TNext>.Some(mapper(maybe.Value))
                : Maybe<TNext>.None;
        }

        public Maybe<TResult> Select<TResult>(Func<TValue, TResult> mapper)
        {
            return maybe.Map(mapper);
        }
    }
}
