using Result.Abstractions;

namespace Result;

/// <summary>
/// Represents the result of an operation that can either be successful or failed.
/// </summary>
public readonly struct Result<TValue> : IResult<TValue>
{
    private readonly TValue? _value;
    private readonly ResultState _state;

    private Result(TValue value)
    {
        _value = value;
        _state = ResultState.Success;
    }

    private Result(IEnumerable<Error> errors)
    {
        Errors = [.. errors];
        _state = ResultState.Faulted;
    }

    private Result(params Error[] errors)
    {
        Errors = [.. errors];
        _state = ResultState.Faulted;
    }

    /// <summary>
    /// Gets the collection of errors if the result is failed.
    /// </summary>
    public Error[] Errors { get; } = [];

    /// <summary>
    /// Gets a value indicating whether returns true if the result is successful, false otherwise.
    /// </summary>
    public bool IsSuccess => _state == ResultState.Success;

    /// <summary>
    /// Gets the value of the result if it is successful.
    /// </summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Result is not successful and value can not be accessed.");

    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return new Result<TValue>(error);
    }

    public static implicit operator Result<TValue>(Error[] failure)
    {
        return new Result<TValue>(failure);
    }

    public static implicit operator bool(Result<TValue> result) => result.IsSuccess;

    /// <summary>
    /// Creates a successful Result with the specified value.
    /// </summary>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Creates a failed Result with the specified error.
    /// </summary>
    public static Result<TValue> Failed(params Error[] errors) => new(errors);

    /// <summary>
    /// Creates a failed Result with the specified collection of errors.
    /// </summary>
    public static Result<TValue> Failed(IEnumerable<Error> errors) => new(errors);

    /// <summary>
    /// Executes a function and catches any exceptions, converting them to a failed Result.
    /// </summary>
    public static Result<T> Try<T>(Func<T> func, Func<Exception, Error>? errorFactory = null)
    {
        try
        {
            return Result<T>.Success(func());
        }
        catch (Exception ex)
        {
            var error = errorFactory?.Invoke(ex) ?? Error.Create("An error occurred during execution", ex);
            return Result<T>.Failed(error);
        }
    }

    /// <summary>
    /// Executes an async function and catches any exceptions, converting them to a failed Result.
    /// </summary>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> func, Func<Exception, Error>? errorFactory = null)
    {
        try
        {
            var result = await func().ConfigureAwait(false);
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            var error = errorFactory?.Invoke(ex) ?? Error.Create("An error occurred during execution", ex);
            return Result<T>.Failed(error);
        }
    }

    /// <summary>
    /// Executes a function that returns a Result and catches any exceptions, converting them to a failed Result.
    /// </summary>
    public static Result<T> TryGet<T>(Func<Result<T>> func, Func<Exception, Error>? errorFactory = null)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            var error = errorFactory?.Invoke(ex) ?? Error.Create("An error occurred during execution", ex);
            return Result<T>.Failed(error);
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
            ? Result<T[]>.Failed(errors)
            : Result<T[]>.Success(values.ToArray());
    }

    public void Deconstruct(out TValue? value, out IEnumerable<Error> errors)
    {
        value = _value;
        errors = Errors;
    }

    public override string ToString()
    {
        return IsSuccess
            ? $"Success: {Value}"
            : $"Failed: {string.Join(", ", Errors.Select(e => e.Message))}";
    }
}

public static class Result
{
    /// <summary>
    /// Creates a successful Result with the specified value.
    /// </summary>
    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);

    /// <summary>
    /// Creates a failed Result with the specified error.
    /// </summary>
    public static Result<TValue> Failed<TValue>(params Error[] errors) => Result<TValue>.Failed(errors);

    /// <summary>
    /// Creates a failed Result with the specified collection of errors.
    /// </summary>
    public static Result<TValue> Failed<TValue>(IEnumerable<Error> errors) => Result<TValue>.Failed(errors);
}
