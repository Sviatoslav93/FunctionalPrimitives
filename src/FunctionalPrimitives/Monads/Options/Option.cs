namespace FunctionalPrimitives.Monads.Options;

/// <summary>
/// Represents an optional value that either contains a value (<see cref="HasValue"/> is <see langword="true"/>)
/// or represents the absence of a value.
/// </summary>
/// <typeparam name="T">The type of the optional value.</typeparam>
public readonly struct Option<T> : IEquatable<Option<T>>
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
    /// Implicitly converts a value to a <see cref="Option{T}"/> containing that value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    public static implicit operator Option<T>(T value) => Some<T>(value);

    /// <summary>
    /// Determines whether two <see cref="Option{T}"/> instances are equal.
    /// </summary>
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Option{T}"/> instances are not equal.
    /// </summary>
    public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);

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

    /// <summary>
    /// Deconstructs this instance into its component parts.
    /// </summary>
    /// <param name="hasValue"><see langword="true"/> if a value is present; otherwise <see langword="false"/>.</param>
    /// <param name="value">The contained value, or <see langword="default"/> when no value is present.</param>
    public void Deconstruct(out bool hasValue, out T? value)
    {
        hasValue = HasValue;
        value = HasValue ? _value : default;
    }

    /// <summary>
    /// Returns a string representation of this instance.
    /// </summary>
    /// <returns><c>"Some(value)"</c> when a value is present; otherwise <c>"None"</c>.</returns>
    public override string ToString() =>
        HasValue ? $"Some({_value})" : "None";

    /// <summary>
    /// Determines whether this instance is equal to another <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="other">The other instance to compare with.</param>
    /// <returns><see langword="true"/> if both instances have the same state and value; otherwise <see langword="false"/>.</returns>
    public bool Equals(Option<T> other)
    {
        if (HasValue != other.HasValue) return false;
        if (!HasValue) return true;

        return EqualityComparer<T>.Default.Equals(_value!, other._value!);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is Option<T> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() =>
        HasValue
            ? HashCode.Combine(true, _value)
            : HashCode.Combine(false);
}
