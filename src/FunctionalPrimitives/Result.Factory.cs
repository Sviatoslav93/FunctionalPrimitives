namespace FunctionalPrimitives;

public static class Result
{
    /// <summary>
    /// Creates a successful <see cref="FunctionalPrimitives{T}"/> with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="value">The value to include in the successful result.</param>
    /// <returns>A successful <see cref="FunctionalPrimitives{T}"/> containing the specified value.</returns>
    public static Result<T> Success<T>(T value) => value;

    public static Result<T> Failure<T>(Error error) => error;

    public static Result<T> Failure<T>(params Error[] errors) => errors;

    /// <summary>
    /// Creates a failed <see cref="Result"/> with the specified errors.
    /// </summary>
    /// <param name="errors">An <see cref="IEnumerable{T}"/> of <see cref="Error"/> used to construct the failed result.</param>
    /// <returns>A failed <see cref="Result"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="errors"/> collection is empty.</exception>
    public static Result<T> Failure<T>(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var array = errors as Error[] ?? [.. errors];

        return array;
    }

    /// <summary>
    /// Executes the provided function and returns a successful <see cref="FunctionalPrimitives{T}"/> containing
    /// the function's return value. If the function throws an exception, the exception is converted
    /// to an <see cref="Error"/> using the supplied <paramref name="errorConvert"/> factory and a failed
    /// <see cref="FunctionalPrimitives{T}"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value produced by <paramref name="func"/>.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="errorConvert">A factory that converts an <see cref="Exception"/> into an <see cref="Error"/>.</param>
    /// <returns>A <see cref="FunctionalPrimitives{T}"/> representing either the successful value or the failure error.</returns>
    public static Result<T> Try<T>(Func<T> func, Func<Exception, Error>? errorConvert = null)
    {
        try
        {
            return Success(func());
        }
        catch (Exception ex)
        {
            return errorConvert == null ? Failure<T>(new Error(ex.Message)) : Failure<T>(errorConvert.Invoke(ex));
        }
    }

    /// <summary>
    /// Executes the provided asynchronous function and wraps its result in a <see cref="FunctionalPrimitives{T}"/>.
    /// If the function throws an exception, the exception is converted into an <see cref="Error"/> using the specified error factory.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the asynchronous function.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="errorConvert">A function to convert exceptions into an <see cref="Error"/> instance.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation that results in a <see cref="FunctionalPrimitives{T}"/>
    /// containing the function's return value, or an error if an exception is thrown.
    /// </returns>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> func, Func<Exception, Error>? errorConvert = null)
    {
        try
        {
            var result = await func().ConfigureAwait(false);
            return Success(result);
        }
        catch (Exception ex)
        {
            return errorConvert == null ? Failure<T>(new Error(ex.Message)) : Failure<T>(errorConvert.Invoke(ex));
        }
    }
}
