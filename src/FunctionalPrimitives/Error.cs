using System.Runtime.CompilerServices;

namespace FunctionalPrimitives;

/// <summary>
/// Represents an error with contextual information such as a message, code,
/// caller member name, source file path, and source line number.
/// </summary>
public record Error(
    string Message,
    string Code = "",
    [CallerMemberName] string MemberName = "not-defined",
    [CallerFilePath] string SourceFilePath = "not-defined",
    [CallerLineNumber] int SourceLineNumber = 0)
{
    public static Error Empty => new Error(string.Empty);
}
