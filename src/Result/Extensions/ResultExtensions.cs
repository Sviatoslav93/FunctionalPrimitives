namespace Result.Extensions;

public static partial class ResultExtensions
{
    public static Result<TValue> AsResult<TValue>(this TValue value) => value;
}
