namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Executes an action if the result is successful, returning the original result.
    /// </summary>
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
        {
            action.Invoke(result.Value);
        }
        return result;
    }

    /// <summary>
    /// Executes an action if the result is failed, returning the original result.
    /// </summary>
    public static Result<T> TapError<T>(this Result<T> result, Action<IEnumerable<Error>> action)
    {
        if (!result.IsSuccess)
        {
            action(result.Errors);
        }
        return result;
    }
}
