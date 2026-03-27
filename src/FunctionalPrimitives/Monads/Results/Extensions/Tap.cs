using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the result is successful.
    /// </summary>
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
    /// Executes the specified action if the result is successful.
    /// </summary>
    public static Result<T> Tap<T>(this Result<T> result, Action action)
    {
        return result.Bind(x =>
        {
            action();
            return x;
        });
    }

    public static Result<T> TapError<T>(this Result<T> result, Action action)
    {
        if (!result.IsSuccess)
        {
            action();
        }

        return result;
    }

    /// <summary>
    /// Executes the specified action if the result contains errors.
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
