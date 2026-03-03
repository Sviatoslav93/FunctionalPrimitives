namespace FunctionalPrimitives;

public static class Maybe
{
    public static Maybe<TValue> Some<TValue>(TValue value)
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value))
            : Maybe<TValue>.Some(value);
    }

    public static Maybe<TValue> None<TValue>()
    {
        return Maybe<TValue>.None;
    }
}
