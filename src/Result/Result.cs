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

    public Error[] Errors { get; } = [];

    public bool IsSuccess => _state == ResultState.Success;

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

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failed(params Error[] errors) => new(errors);

    public static Result<TValue> Failed(IEnumerable<Error> errors) => new(errors);

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
