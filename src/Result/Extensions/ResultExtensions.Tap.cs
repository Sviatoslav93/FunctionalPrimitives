namespace Result.Extensions;

public partial class ResultExtensions
{
    /// <summary>
    /// Executes an action if the result is successful.
    /// </summary>
    /// <returns> original result. </returns>
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        return result.Match(
            x =>
            {
                action(x);
                return x;
            },
            _ => result);
    }

    /// <summary>
    /// Executes an action if the result is successful.
    /// </summary>
    /// <returns> original result. </returns>
    public static Result<T> Tap<T>(
        this Result<T> result,
        Action action)
    {
        return result.Then(x =>
        {
            action();
            return x;
        });
    }

    /// <summary>
    /// Executes an action if the result is failed.
    /// </summary>
    /// <returns> original result. </returns>
    public static Result<T> TapError<T>(this Result<T> result, Action<IEnumerable<Error>> action)
    {
        if (!result.IsSuccess)
        {
            action(result.Errors);
        }

        return result;
    }
}
