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
    public static Result<T> ToResult<T>(this T? nullable, Error? error = null)
        where T : struct
    {
        if (nullable.HasValue)
        {
            return Result<T>.Success(nullable.Value);
        }

        var errorToUse = error ?? Error.Create("Value is null");
        return Result<T>.Failed(errorToUse);
    }

    /// <summary>
    /// Converts a nullable reference to a Result.
    /// </summary>
    public static Result<T> ToResult<T>(this T? nullable, Error? error = null)
        where T : class
    {
        if (nullable is not null)
        {
            return Result<T>.Success(nullable);
        }

        var errorToUse = error ?? Error.Create("Value is null");
        return Result<T>.Failed(errorToUse);
    }

    /// <summary>
    /// Gets the value or returns a default value if the result is failed.
    /// </summary>
    public static T GetValueOrDefault<T>(this Result<T> result, T defaultValue = default!)
    {
        return result.IsSuccess ? result.Value : defaultValue;
    }

    /// <summary>
    /// Gets the value or returns the result of a factory function if the result is failed.
    /// </summary>
    public static T GetValueOrDefault<T>(this Result<T> result, Func<T> defaultFactory)
    {
        return result.IsSuccess ? result.Value : defaultFactory();
    }

    /// <summary>
    /// Gets the value or returns the result of a factory function that receives the errors.
    /// </summary>
    public static T GetValueOrDefault<T>(this Result<T> result, Func<IEnumerable<Error>, T> defaultFactory)
    {
        return result.IsSuccess ? result.Value : defaultFactory(result.Errors);
    }

    /// <summary>
    /// Throws an exception if the result is failed.
    /// </summary>
    public static T GetValueOrThrow<T>(this Result<T> result, Func<IEnumerable<Error>, Exception>? exceptionFactory = null)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        var exception = exceptionFactory?.Invoke(result.Errors) ??
                       new InvalidOperationException($"Result failed with errors: {string.Join(", ", result.Errors.Select(e => e.Message))}");
        throw exception;
    }
}
