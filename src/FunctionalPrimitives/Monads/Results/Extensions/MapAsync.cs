namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Asynchronously transforms the value of a successful result using the specified mapping function.
    /// </summary>
    public static async Task<Result<U>> MapAsync<T, U>(this Task<Result<T>> resultTask, Func<T, U> projection)
    {
        var result = await resultTask.ConfigureAwait(false);

        return result.IsSuccess
            ? projection(result.Value)
            : Failure<U>(result.ErrorsInternal);
    }

    /// <summary>
    /// Asynchronously projects the value of a successful result using LINQ query syntax.
    /// </summary>
    public static Task<Result<U>> Select<T, U>(this Task<Result<T>> resultTask, Func<T, U> projection)
        => resultTask.MapAsync(projection);
}
