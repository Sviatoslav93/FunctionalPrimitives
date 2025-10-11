namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines multiple async results into a single result.
    /// </summary>
    public static async Task<Result<T[]>> CombineAsync<T>(params Task<Result<T>>[] tasks)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return Combine(results);
    }

    /// <summary>
    /// Combines multiple async results into a single result.
    /// </summary>
    public static async Task<Result<T[]>> CombineAsync<T>(this IEnumerable<Task<Result<T>>> tasks)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return Combine(results);
    }
}
