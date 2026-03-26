namespace FunctionalPrimitives.Errors.Extensions;

/// <summary>
/// Provides extension methods for enriching <see cref="Error"/> instances with additional metadata.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    /// Returns a copy of the error with an additional metadata entry added or updated.
    /// </summary>
    /// <param name="error">The source error to enrich.</param>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value to associate with the key.</param>
    /// <returns>A new <see cref="Error"/> instance that is identical to the original but with the specified metadata entry set.</returns>
    public static Error WithMetadata(
        this Error error,
        string key,
        object value)
    {
        var metadata = error.Metadata is null
            ? []
            : new Dictionary<string, object>(error.Metadata);

        metadata[key] = value;

        return error with { Metadata = metadata };
    }
}
