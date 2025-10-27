namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines multiple <see cref="Result{T}"/> objects into a single result.
    /// If all the results are successful, the combined result will also be successful,
    /// containing an array of all successful values. If any result fails, the combined
    /// result will be a failure and will aggregate all errors.
    /// </summary>
    /// <typeparam name="T">The type of the values contained in the results.</typeparam>
    /// <param name="results">An enumerable collection of <see cref="Result{T}"/> objects to be combined.</param>
    /// <returns>
    /// A Result containing an array of all successful values where:
    /// - If all input results are successful, the result will be successful and contain an array of their values.
    /// - If any input result is a failure, the result will be a failure and will aggregate all the errors.
    /// </returns>
    public static Result<T[]> Combine<T>(this IEnumerable<Result<T>> results)
    {
        var resultArray = results.ToArray();
        var errors = new List<Error>();
        var values = new List<T>();

        foreach (var result in resultArray)
        {
            if (result.IsSuccess)
            {
                values.Add(result.Value);
            }
            else
            {
                errors.AddRange(result.Errors);
            }
        }

        return errors.Count > 0
            ? Result.Failure<T[]>(errors)
            : Result.Success(values.ToArray());
    }

    /// <summary>
    /// Combines the specified result with an array of additional results into a single result.
    /// If all results are successful, the combined result will also be successful, containing an array of their values.
    /// If any result fails, the combined result will be a failure and will aggregate all the errors.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The first <see cref="Result{T}"/> to be included in the combination.</param>
    /// <param name="results">An array of <see cref="Result{T}"/> objects to be combined together with the first result.</param>
    /// <returns>
    /// A Result containing an array of all successful values where:
    /// - If all input results are successful, the result will be successful and contain an array of their values.
    /// - If any input result is a failure, the result will be a failure and will aggregate all the errors.
    /// </returns>
    public static Result<T[]> Combine<T>(
        this Result<T> result,
        params Result<T>[] results)
    {
        List<Result<T>> arr = [result, .. results];

        return arr.Combine();
    }
}
