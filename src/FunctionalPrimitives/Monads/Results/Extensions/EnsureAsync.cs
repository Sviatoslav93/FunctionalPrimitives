using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result<T>> EnsureAsync<T>(this T value, Func<T, Task<bool>> predicate, Error error)
    {
        return await predicate(value).ConfigureAwait(false) ? value : error;
    }

    public static async Task<Result<T>> EnsureAsync<T>(this T value, Func<T, Task<bool>> predicate, Func<T, Error> error)
    {
        return await predicate(value).ConfigureAwait(false) ? value : error(value);
    }

    public static Task<Result<T>> EnsureAsync<T>(this Task<Result<T>> result, Func<T, bool> predicate, Error error)
    {
        return result.MatchAsync(
            x => predicate(x) ? Success(x) : error,
            errors => Failure<T>(errors.ToArray()));
    }

    public static Task<Result<T>> EnsureAsync<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Error> error)
    {
        return result.MatchAsync(
            x => predicate(x) ? Success(x) : error(x),
            errors => Failure<T>(errors.ToArray()));
    }
}
