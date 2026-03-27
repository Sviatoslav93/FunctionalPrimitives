using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms a <see cref="Result{TValue}"/> into a value of type <typeparamref name="TNext"/>
    /// using specified functions for success and failure scenarios.
    /// </summary>
    public static TNext Match<TValue, TNext>(
        this Result<TValue> result,
        Func<TValue, TNext> onSuccess,
        Func<IEnumerable<Error>, TNext> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Errors);
    }
}
