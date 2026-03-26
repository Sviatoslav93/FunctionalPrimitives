namespace FunctionalPrimitives.Monads.Options;

/// <summary>
/// Provides factory methods for creating <see cref="Option{T}"/> instances.
/// </summary>
public static class Option
{
    /// <summary>
    /// Creates a <see cref="Option{T}"/> containing the specified non-null value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="Option{T}"/> containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    public static Option<T> Some<T>(T value)
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value))
            : Option<T>.Some(value);
    }

    /// <summary>
    /// Creates a <see cref="Option{T}"/> representing the absence of a value.
    /// </summary>
    /// <typeparam name="T">The type of the absent value.</typeparam>
    /// <returns>A <see cref="Option{T}"/> with no value present.</returns>
    public static Option<T> None<T>()
    {
        return default;
    }
}
