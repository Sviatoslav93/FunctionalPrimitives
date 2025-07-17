namespace Result;

public interface IResult
{
    bool IsSuccess { get; }
}

public interface IResult<out T> : IResult
{
    T? Value { get; }
}
