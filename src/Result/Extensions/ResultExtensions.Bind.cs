namespace Result.Extensions;

/// <summary>
/// Provides extension methods for transforming and handling results in a functional programming style.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Binds the result of a successful operation to a new result by applying the specified transformation function.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the initial result.</typeparam>
    /// <typeparam name="TNextValue">The type of the value for the new result.</typeparam>
    /// <param name="result">The original result to process and transform.</param>
    /// <param name="onSuccess">A function to transform the value of a successful result into a new value or result.</param>
    /// <returns>
    /// If the input result is successful, returns a new result produced by the transformation function.
    /// If the input result is a failure, returns the existing errors unchanged.
    /// </returns>
    public static Result<TNextValue> Bind<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, TNextValue> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : result.Errors;
    }

    /// <summary>
    /// Transforms a successful result into a new result by applying the provided transformation function.
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in the initial result.</typeparam>
    /// <typeparam name="TNextValue">The type of the value for the new result.</typeparam>
    /// <param name="result">The original result to process and transform.</param>
    /// <param name="onSuccess">A function that transforms the value of a successful result into a new result.</param>
    /// <returns>
    /// A new result produced by the transformation function if the initial result is successful.
    /// If the initial result is a failure, the existing errors are returned unchanged.
    /// </returns>
    public static Result<TNextValue> Bind<TValue, TNextValue>(
        this Result<TValue> result,
        Func<TValue, Result<TNextValue>> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : result.Errors;
    }
}
