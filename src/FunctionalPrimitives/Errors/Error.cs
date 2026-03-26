namespace FunctionalPrimitives.Errors;

/// <summary>
/// Represents an error with a message and optional code.
/// </summary>
public record Error(
    string Message,
    string Code = "")
{
    public static readonly Error Empty = new(string.Empty);

    public IReadOnlyDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();

    public virtual string Type => "default";

    public static implicit operator Error(string message) => new(message);
}
