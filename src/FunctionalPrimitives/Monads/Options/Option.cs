namespace FunctionalPrimitives.Monads.Options;

/// <summary>
/// Represents an optional value that either contains a value (<see cref="HasValue"/> is <see langword="true"/>)
/// or represents the absence of a value.
/// </summary>
/// <typeparam name="T">The type of the optional value.</typeparam>
public readonly struct Option<T>
{
    private readonly T _value;

    /// <summary>
    /// Initializes a new instance of <see cref="Option{T}"/> representing no value.
    /// </summary>
    public Option()
    {
        _value = default!;
        HasValue = false;
    }

    private Option(T value)
    {
        _value = value;
        HasValue = true;
    }

    /// <summary>
    /// Gets a value indicating whether this instance contains a value.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Gets the value contained in this instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when accessed on an instance with no value present.</exception>
    public T Value =>
        HasValue
            ? _value!
            : throw new InvalidOperationException("No value present.");

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> containing the specified non-null value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="Option{T}"/> containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    public static Option<TValue> Some<TValue>(TValue value)
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value))
            : new Option<TValue>(value);
    }

    /// <summary>
    /// Returns the contained value if present; otherwise returns <paramref name="defaultValue"/>.
    /// </summary>
    /// <param name="defaultValue">The fallback value to return when no value is present.</param>
    /// <returns>The contained value, or <paramref name="defaultValue"/> when <see cref="HasValue"/> is <see langword="false"/>.</returns>
    public T GetValueOrDefault(T defaultValue) =>
        HasValue ? _value! : defaultValue;
}
