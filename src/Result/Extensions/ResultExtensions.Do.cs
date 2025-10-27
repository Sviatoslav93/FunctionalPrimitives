namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the result is successful.
    /// If the result is a failure, execution of the action is skipped, and the original result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value contained within the result.</typeparam>
    /// <param name="result">The result object on which the operation is performed.</param>
    /// <param name="action">The action to execute when the result is successful.</param>
    /// <returns>The original result, whether the action is executed or not.</returns>
    public static Result<T> Do<T>(
        this Result<T> result,
        Action<T> action)
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
    /// Executes the specified action if the result is successful.
    /// If the result is a failure, the action is not executed, and the original result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value contained within the result.</typeparam>
    /// <param name="result">The result instance to evaluate and potentially execute the action on.</param>
    /// <param name="action">The action to execute when the result is successful.</param>
    /// <returns>The original result, whether the action is executed or not.</returns>
    public static Result<T> Do<T>(
        this Result<T> result,
        Action action)
    {
        return result.Bind(x =>
        {
            action();
            return x;
        });
    }

    public static Result<T> DoError<T>(
        this Result<T> result,
        Action<IEnumerable<Error>> action)
    {
        if (!result.IsSuccess)
        {
            action(result.Errors);
        }

        return result;
    }
}
