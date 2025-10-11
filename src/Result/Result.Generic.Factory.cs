namespace Result;

public partial class Result
{
    /// <summary>
    /// Creates a successful <see cref="Result&lt;T&gt;"/> with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="value">The value to include in the successful result.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the specified value.</returns>
    public static Result<T> Success<T>(T value) => value;

    public static Result<T> Failure<T>(Error error) => error;

    public static Result<T> Failure<T>(params Error[] errors) => errors;

    /// <summary>
    /// Creates a failed <see cref="Result"/> with the specified errors.
    /// </summary>
    /// <param name="errors">An <see cref="IEnumerable&lt;T&gt;"/> of <see cref="Error"/> used to construct the failed result.</param>
    /// <returns>A failed <see cref="Result"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="errors"/> collection is empty.</exception>
    public static Result<T> Failure<T>(IEnumerable<Error> errors)
    {
        var enumerable = errors as Error[] ?? [.. errors];

        if (errors == null || enumerable.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        return enumerable.ToArray();
    }

    public static Result<T> Try<T>(Func<T> func, Func<Exception, Error> error)
    {
        try
        {
            return Success(func());
        }
        catch (Exception ex)
        {
            return Failure<T>(error.Invoke(ex));
        }
    }

    /// <summary>
    /// Executes an async function and catches any exceptions, converting them to a failed Result.
    /// </summary>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> func, Func<Exception, Error> errorFactory)
    {
        try
        {
            var result = await func().ConfigureAwait(false);
            return Success(result);
        }
        catch (Exception ex)
        {
            return Failure<T>(errorFactory.Invoke(ex));
        }
    }

    /// <summary>
    /// Executes a function that returns a Result and catches any exceptions, converting them to a failed Result.
    /// </summary>
    public static Result<T> TryGet<T>(Func<Result<T>> func, Func<Exception, Error> errorFactory)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            return Failure<T>(errorFactory.Invoke(ex));
        }
    }

    /// <summary>
    /// Combines multiple results into a single result containing all values or all errors.
    /// </summary>
    public static Result<T[]> Combine<T>(params Result<T>[] results)
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
            ? Failure<T[]>(errors)
            : Success(values.ToArray());
    }
}
