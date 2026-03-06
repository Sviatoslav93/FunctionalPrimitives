namespace FunctionalPrimitives;

/// <summary>
/// Provides factory methods for creating <see cref="Maybe{T}"/> instances.
/// </summary>
public static class Maybe
{
    /// <summary>
    /// Creates a <see cref="Maybe{T}"/> containing the specified non-null value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="Maybe{T}"/> containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    public static Maybe<T> Some<T>(T value)
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value))
            : Maybe<T>.Some(value);
    }

    /// <summary>
    /// Creates a <see cref="Maybe{T}"/> representing the absence of a value.
    /// </summary>
    /// <typeparam name="T">The type of the absent value.</typeparam>
    /// <returns>A <see cref="Maybe{T}"/> with no value present.</returns>
    public static Maybe<T> None<T>()
    {
        return default;
    }
}
