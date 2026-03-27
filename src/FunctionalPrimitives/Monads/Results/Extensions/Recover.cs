using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Recovers a failed result by providing a fallback value to use in case of failure.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, T fallback)
    {
        return result.Match(
            x => x,
            _ => Success(fallback));
    }

    /// <summary>
    /// Recovers a failed result by using a factory function to generate a fallback value.
    /// </summary>
    public static Result<T> Recover<T>(this Result<T> result, Func<IEnumerable<Error>, T> recover)
    {
        return result.Match(
            x => x,
            err => Success(recover(err)));
    }
}
