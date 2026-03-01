namespace FunctionalPrimitives.Extensions.Result;

public static partial class ResultExtensions
{
    /// <summary>
    /// Provides extension methods for working with <see cref="FunctionalPrimitives{T}"/> objects.
    /// </summary>
    extension<TValue>(IEnumerable<Result<TValue>> results)
    {
        /// <summary>
        /// Combines multiple <see cref="FunctionalPrimitives{TValue}"/> objects into a single result.
        /// If all results are successful, the combined result will contain an array of their values as a successful result.
        /// If any result is a failure, the combined result will be a failure and will aggregate all encountered errors.
        /// </summary>
        /// <returns>
        /// - If all input results are successful, it will be successful and contain an array of their values.
        /// - If any input result is a failure, it will be a failure containing all the aggregated errors.
        /// </returns>
        public Result<TValue[]> Combine()
        {
            var resultArray = results.ToArray();
            var errors = new List<Error>();
            var values = new List<TValue>();

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
                ? FunctionalPrimitives.Result.Failure<TValue[]>(errors)
                : FunctionalPrimitives.Result.Success(values.ToArray());
        }
    }

    /// <summary>
    /// Combines the specified result with an array of additional results into a single result.
    /// If all results are successful, the combined result will also be successful, containing an array of their values.
    /// If any result fails, the combined result will be a failure and will aggregate all the errors.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The first <see cref="FunctionalPrimitives{T}"/> to be included in the combination.</param>
    /// <param name="results">An array of <see cref="FunctionalPrimitives{T}"/> objects to be combined together with the first result.</param>
    /// <returns>
    /// A FunctionalPrimitives containing an array of all successful values where:
    /// - If all input results are successful, the result will be successful and contain an array of their values.
    /// - If any input result is a failure, the result will be a failure and will aggregate all the errors.
    /// </returns>
    public static Result<T[]> Combine<T>(
        this Result<T> result,
        params Result<T>[] results)
    {
        // todo: fix with extension c#14 syntax, currently it leads to build errors
        List<Result<T>> arr = [result, .. results];

        return arr.Combine();
    }
}
