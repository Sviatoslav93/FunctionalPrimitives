namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines multiple results into a single result containing all values or all errors.
    /// </summary>
    public static Result<T[]> Combine<T>(this IEnumerable<Result<T>> results)
    {
        return Combine(results.ToArray());
    }
}
