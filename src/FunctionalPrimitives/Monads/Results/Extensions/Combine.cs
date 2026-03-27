using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines multiple <see cref="Result{T}"/> objects into a single result.
    /// </summary>
    public static Result<T[]> Combine<T>(this IEnumerable<Result<T>> results)
    {
        var errors = new List<Error>();
        var values = new List<T>();

        foreach (var result in results)
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
            ? Failure<T[]>(errors.ToArray())
            : Success(values.ToArray());
    }

    public static Result<T[]> Combine<T>(
        this Result<T> result,
        params Result<T>[] results)
    {
        List<Result<T>> arr = [result, .. results];

        return arr.Combine();
    }
}
