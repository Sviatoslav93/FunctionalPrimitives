namespace Result;

/// <summary>
/// Represents the result of an operation, indicating success or failure.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets the collection of errors encountered during the operation.
    /// </summary>
    IReadOnlyCollection<Error> Errors { get; }
}
