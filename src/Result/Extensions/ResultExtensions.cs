namespace Result.Extensions;

public static partial class ResultExtensions
{
    /// <summary>
    /// Converts the provided value into a <see cref="Result{TValue}"/> object, representing a successful result.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to be wrapped in the result.</typeparam>
    /// <param name="value">The value to wrap in a successful <see cref="Result{TValue}"/>.</param>
    /// <returns>A <see cref="Result{TValue}"/> representing a successful operation containing the specified value.</returns>
    public static Result<TValue> AsResult<TValue>(this TValue value) => value;
}
