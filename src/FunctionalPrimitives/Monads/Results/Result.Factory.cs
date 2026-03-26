using FunctionalPrimitives.Abstractions;
using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results;

/// <summary>
/// Provides factory and utility methods for creating and combining <see cref="Result{T}"/> instances.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="value">The value to include in the successful result.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the specified value.</returns>
    public static Result<T> Success<T>(T value) => value;

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> of type <see cref="Unit"/>,
    /// representing a successful operation that produces no value.
    /// </summary>
    /// <returns>A successful <see cref="Result{T}"/> containing <see cref="Unit.Value"/>.</returns>
    public static Result<Unit> Success() => Unit.Value;

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the specified error.
    /// </summary>
    /// <typeparam name="T">The type of the value that would have been contained in a successful result.</typeparam>
    /// <param name="error">The <see cref="Error"/> that describes the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the specified error.</returns>
    public static Result<T> Failure<T>(Error error)
    {
        return Failure<T>([error]);
    }

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the specified errors.
    /// If no errors are provided, a result containing <see cref="Error.Empty"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value that would have been contained in a successful result.</typeparam>
    /// <returns>A failed <see cref="Result{T}"/> containing the provided errors, or <see cref="Error.Empty"/> if none were supplied.</returns>
    public static Result<T> Failure<T>(IResult result)
    {
        return result.IsSuccess
            ? throw new ArgumentException("Result must be a failure", nameof(result))
            : new Result<T>(result.Errors.ToArray());
    }

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> with the specified errors.
    /// If no errors are provided, a result containing <see cref="Error.Empty"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value that would have been contained in a successful result.</typeparam>
    /// <param name="errors">One or more <see cref="Error"/> instances that describe the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the provided errors, or <see cref="Error.Empty"/> if none were supplied.</returns>
    public static Result<T> Failure<T>(params Error[] errors)
    {
        if (errors.Length == 0)
        {
            errors = [Error.Empty];
        }

        return new Result<T>(errors);
    }

    /// <summary>
    /// Executes the provided function and returns a successful <see cref="Result{T}"/> containing
    /// the function's return value. If the function throws an exception, the exception is converted
    /// to an <see cref="Error"/> using the supplied <paramref name="errorConvert"/> factory and a failed
    /// <see cref="Result{T}"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value produced by <paramref name="func"/>.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="errorConvert">A factory that converts an <see cref="System.Exception"/> into an <see cref="Error"/>.</param>
    /// <returns>A <see cref="Result{T}"/> representing either the successful value or the failure error.</returns>
    public static Result<T> Try<T>(Func<T> func, Func<System.Exception, Error>? errorConvert = null)
    {
        try
        {
            return Success(func());
        }
        catch (System.Exception ex)
        {
            return errorConvert == null ? Failure<T>(new Error(ex.Message)) : Failure<T>(errorConvert.Invoke(ex));
        }
    }

    /// <summary>
    /// Executes the provided asynchronous function and wraps its result in a <see cref="Result{T}"/>.
    /// If the function throws an exception, the exception is converted into an <see cref="Error"/> using the specified error factory.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the asynchronous function.</typeparam>
    /// <param name="func">The asynchronous function to execute.</param>
    /// <param name="errorConvert">A function to convert exceptions into an <see cref="Error"/> instance.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation that results in a <see cref="Result{T}"/>
    /// containing the function's return value, or an error if an exception is thrown.
    /// </returns>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> func, Func<System.Exception, Error>? errorConvert = null)
    {
        try
        {
            var result = await func().ConfigureAwait(false);
            return Success(result);
        }
        catch (System.Exception ex)
        {
            return errorConvert == null ? Failure<T>(new Error(ex.Message)) : Failure<T>(errorConvert.Invoke(ex));
        }
    }

    /// <summary>
    /// Combines multiple <see cref="IResult"/> instances into a single <see cref="Result{T}"/> of type <see cref="Unit"/>.
    /// Returns a successful result if all inputs are successful; otherwise aggregates all errors into a single failure.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> of <see cref="Unit"/> if all <paramref name="results"/> are successful;
    /// otherwise a failed result containing every error from every failed input.
    /// </returns>
    public static Result<Unit> Combine(params IResult[] results)
    {
        var errors = results
            .Where(x => !x.IsSuccess)
            .SelectMany(x => x.Errors)
            .ToArray();

        return errors.Length == 0
            ? Success()
            : Failure<Unit>(errors);
    }
}
