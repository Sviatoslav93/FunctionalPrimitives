namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms the value of a successful result using the specified mapping function.
    /// </summary>
    public static Result<U> Map<T, U>(this Result<T> result, Func<T, U> mapper)
    {
        return result.IsSuccess
            ? mapper(result.Value)
            : Failure<U>(result.ErrorsInternal);
    }

    /// <summary>
    /// Projects the value of a successful result using LINQ query syntax.
    /// </summary>
    public static Result<U> Select<T, U>(this Result<T> result, Func<T, U> selector)
        => result.Map(selector);
}
