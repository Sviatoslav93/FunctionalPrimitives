namespace Result.Extensions;

public static partial class ResultExtensions
{
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
}
