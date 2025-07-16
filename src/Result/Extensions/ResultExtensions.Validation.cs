namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Ensures that a value satisfies a condition, otherwise returns a failed result.
    /// </summary>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (!result.IsSuccess)
            return result;

        return predicate(result.Value) ? result : Result.Failed<T>(error);
    }

    /// <summary>
    /// Ensures that a value satisfies a condition, otherwise returns a failed result.
    /// </summary>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Func<T, Error> errorFactory)
    {
        if (!result.IsSuccess)
            return result;

        return predicate(result.Value) ? result : Result.Failed<T>(errorFactory(result.Value));
    }

    /// <summary>
    /// Ensures that multiple conditions are satisfied.
    /// </summary>
    public static Result<T> EnsureAll<T>(this Result<T> result, params (Func<T, bool> predicate, Error error)[] validations)
    {
        if (!result.IsSuccess)
            return result;

        var errors = new List<Error>();
        var value = result.Value;

        foreach (var (predicate, error) in validations)
        {
            if (!predicate(value))
            {
                errors.Add(error);
            }
        }

        return errors.Count > 0 ? Result.Failed<T>(errors) : result;
    }

    /// <summary>
    /// Validates that a value is not null.
    /// </summary>
    public static Result<T> EnsureNotNull<T>(this Result<T?> result, Error? error = null) where T : class
    {
        if (!result.IsSuccess)
            return Result.Failed<T>(result.Errors);

        if (result.Value is null)
        {
            var errorToUse = error ?? Error.Create("Value cannot be null");
            return Result.Failed<T>(errorToUse);
        }

        return Result.Success(result.Value);
    }

    /// <summary>
    /// Validates that a nullable value is not null.
    /// </summary>
    public static Result<T> EnsureNotNull<T>(this Result<T?> result, Error? error = null) where T : struct
    {
        if (!result.IsSuccess)
            return Result.Failed<T>(result.Errors);

        if (!result.Value.HasValue)
        {
            var errorToUse = error ?? Error.Create("Value cannot be null");
            return Result.Failed<T>(errorToUse);
        }

        return Result.Success(result.Value.Value);
    }

    /// <summary>
    /// Validates that a string is not null or empty.
    /// </summary>
    public static Result<string> EnsureNotNullOrEmpty(this Result<string?> result, Error? error = null)
    {
        if (!result.IsSuccess)
            return Result.Failed<string>(result.Errors);

        if (string.IsNullOrEmpty(result.Value))
        {
            var errorToUse = error ?? Error.Create("String cannot be null or empty");
            return Result.Failed<string>(errorToUse);
        }

        return Result.Success(result.Value);
    }

    /// <summary>
    /// Validates that a string is not null, empty, or whitespace.
    /// </summary>
    public static Result<string> EnsureNotNullOrWhiteSpace(this Result<string?> result, Error? error = null)
    {
        if (!result.IsSuccess)
            return Result.Failed<string>(result.Errors);

        if (string.IsNullOrWhiteSpace(result.Value))
        {
            var errorToUse = error ?? Error.Create("String cannot be null, empty, or whitespace");
            return Result.Failed<string>(errorToUse);
        }

        return Result.Success(result.Value);
    }
}
