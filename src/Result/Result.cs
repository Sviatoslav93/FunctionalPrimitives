namespace Result;

public partial class Result
{
    private readonly ResultState _state;

    protected Result(params Error[] errors)
    {
        Errors = [.. errors];
        _state = errors.Length > 0 ? ResultState.Faulted : ResultState.Success;
    }

    public Error[] Errors { get; }

    public bool IsSuccess => _state == ResultState.Success;

    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }

    public static implicit operator Result(Error[] failure)
    {
        return new Result(failure);
    }

    public static implicit operator bool(Result result) => result.IsSuccess;
}
