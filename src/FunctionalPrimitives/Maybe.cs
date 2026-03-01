namespace FunctionalPrimitives.Maybe;

public readonly struct Maybe<T>
{
    private readonly T _value;

    private Maybe(T value)
    {
        _value = value;
        HasValue = true;
    }

    public bool HasValue { get; }

    public bool HasNoValue => !HasValue;

    public T Value =>
        HasValue
            ? _value!
            : throw new InvalidOperationException("No value present.");

#pragma warning disable SA1204
    public static Maybe<T> None { get; } = default;
#pragma warning restore SA1204

    public static Maybe<T> Some(T value)
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value))
            : new Maybe<T>(value);
    }

    public T GetValueOrDefault(T defaultValue) =>
        HasValue ? _value! : defaultValue;

    public TResult Match<TResult>(
        Func<T, TResult> onSome,
        Func<TResult> onNone)
    {
        return HasValue
            ? onSome(_value!)
            : onNone();
    }
}
