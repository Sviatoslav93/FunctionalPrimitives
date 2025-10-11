namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Tries to recover from a failed result by executing an async recovery function.
    /// </summary>
    public static async Task<Result<T>> RecoverAsync<T>(this Result<T> result, Func<IEnumerable<Error>, Task<T>> recovery)
    {
        if (result.IsSuccess)
            return result;

        var recoveredValue = await recovery(result.Errors).ConfigureAwait(false);
        return Result.Success(recoveredValue);
    }

    /// <summary>
    /// Tries to recover from a failed result by executing an async recovery function that returns a Result.
    /// </summary>
    public static Task<Result<T>> RecoverAsync<T>(
        this Result<T> result,
        Func<IEnumerable<Error>, Task<Result<T>>> recovery)
    {
        return result.MatchAsync(
            x => x,
            recovery);
    }

    /// <summary>
    /// Tries to recover from a failed task result by executing an async recovery function.
    /// </summary>
    public static Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> task,
        Func<IEnumerable<Error>, Task<Result<T>>> recovery)
    {
        return task.MatchAsync(
            x => x,
            recovery);
    }
}
