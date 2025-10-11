namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Converts a Result to an Optional/Maybe-like structure.
    /// </summary>
    public static T? ToNullable<T>(this Result<T> result)
        where T : struct
    {
        return result.IsSuccess ? result.Value : null;
    }

    /// <summary>
    /// Converts a Result to a nullable reference type.
    /// </summary>
    public static T? ToNullableReference<T>(this Result<T> result)
        where T : class
    {
        return result.IsSuccess ? result.Value : null;
    }

    /// <summary>
    /// Converts a nullable value to a Result.
    /// </summary>
    public static Result<T> ToResult<T>(this T? nullable, Error error)
        where T : struct
    {
        return nullable.HasValue
            ? Result.Success(nullable.Value)
            : Result.Failure<T>(error);
    }

    /// <summary>
    /// Converts a nullable reference to a Result.
    /// </summary>
    public static Result<T> ToResult<T>(this T? nullable, Error error)
        where T : class
    {
        return nullable is not null
            ? Result.Success(nullable)
            : Result.Failure<T>(error);
    }

    /// <summary>
    /// Gets the value or returns a default value if the result is failed.
    /// </summary>
    public static T GetValueOrDefault<T>(this Result<T> result, T defaultValue = default!)
    {
        return result.Match(
            x => x,
            _ => defaultValue);
    }

    /// <summary>
    /// Gets the value or returns the result of a factory function that receives the errors.
    /// </summary>
    public static T GetValueOrDefault<T>(this Result<T> result, Func<IEnumerable<Error>, T> factory)
    {
        return result.Match(
            x => x,
            err => factory(err));
    }

    /// <summary>
    /// Throws an exception if the result is failed.
    /// </summary>
    public static T GetValueOrThrow<T>(this Result<T> result, Func<IEnumerable<Error>, Exception> exceptionFactory)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        var exception = exceptionFactory.Invoke(result.Errors);
        throw exception;
    }
}
