namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes one of the provided functions depending on the state of the result (success or failure),
    /// and returns a value of type <typeparamref name="TNext"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the successful result.</typeparam>
    /// <typeparam name="TNext">The type of the value returned by the matched function.</typeparam>
    /// <param name="result">The result to match on, which determines whether to invoke <paramref name="onSuccess"/> or <paramref name="onFailure"/>.</param>
    /// <param name="onSuccess">A function to process the value of a successful result and return a value of type <typeparamref name="TNext"/>.</param>
    /// <param name="onFailure">A function to process the errors of a failed result and return a value of type <typeparamref name="TNext"/>.</param>
    /// <returns>
    /// The value returned by either the <paramref name="onSuccess"/> function for a successful result,
    /// or the <paramref name="onFailure"/> function for a failed result.
    /// </returns>
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
