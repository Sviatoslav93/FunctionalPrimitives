namespace FunctionalPrimitives;

public static class Maybe
{
    public static Maybe<T> Some<T>(T value)
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value))
            : Maybe<T>.Some(value);
    }

    public static Maybe<T> None<T>()
    {
        return default;
    }
}
