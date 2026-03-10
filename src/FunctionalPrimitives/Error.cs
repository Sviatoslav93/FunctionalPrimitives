namespace FunctionalPrimitives;

/// <summary>
/// Represents an error with a message and optional code.
/// </summary>
public readonly record struct Error(
    string Message,
    string Code = "",
    string Type = "",
    IReadOnlyDictionary<string, object>? Metadata = null)
{
    public static readonly Error Empty = new(string.Empty);

    public static implicit operator Error(string message) => new(message);

    public static implicit operator Error(Exception ex) => new(ex.Message, ex.GetType().Name, nameof(Exception));

    public static Error FromCode(string code, string message, string type = "") => new(message, code, type);
}
