namespace Result;

public class Result<T> : Result
{
    private readonly T? _value;

    protected Result(T value)
    {
        _value = value;
    }

    protected Result(params Error[] errors)
        : base(errors)
    {
    }

    /// <summary>
    /// Gets the value of the result if it is successful.
    /// </summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Result is not successful and value can not be accessed.");

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }

    public static implicit operator Result<T>(Error[] failure)
    {
        return new Result<T>(failure);
    }

    public void Deconstruct(out T? value, out IEnumerable<Error> errors)
    {
        value = _value;
        errors = Errors;
    }
}
