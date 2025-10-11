using System.Runtime.CompilerServices;

namespace Result;

/// <summary>
/// Result error.
/// </summary>
public class Error(
    string message,
    string code = "",
    [CallerMemberName] string memberName = "not-defined",
    [CallerFilePath] string sourceFilePath = "not-defined",
    [CallerLineNumber] int sourceLineNumber = 0)
{
    public string Message { get; } = message;

    public string Code { get; } = code;

    public string MemberName { get; } = memberName;

    public string SourceFilePath { get; } = sourceFilePath;

    public int SourceLineNumber { get; } = sourceLineNumber;
}
