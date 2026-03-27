using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;

namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Converts this nullable reference to a <see cref="Option{T}"/>.
    /// Returns <see cref="Option{T}"/> with a value if non-null; otherwise returns <see cref="Option{T}"/> with no value.
    /// </summary>
    public static Option<T> ToMaybe<T>(this T? obj)
        where T : class
        => obj is null ? None<T>() : Some(obj);

    /// <summary>
    /// Converts this nullable value type to a <see cref="Option{T}"/>.
    /// Returns <see cref="Option{T}"/> with a value if it has one; otherwise returns an empty <see cref="Option{T}"/>.
    /// </summary>
    public static Option<T> ToMaybe<T>(this T? value)
        where T : struct
        => value.HasValue ? Some(value.Value) : None<T>();

    /// <summary>
    /// Converts this <see cref="Option{T}"/> to a <see cref="Result{T}"/>.
    /// Returns a successful result if a value is present; otherwise returns a failed result with the specified error.
    /// </summary>
    public static Result<T> ToResult<T>(this Option<T> option, Error error)
        => option.Match(x => x, () => Failure<T>(error));

    /// <summary>
    /// Returns the contained value if present; otherwise returns the specified fallback value.
    /// </summary>
    public static T GetValueOr<T>(this Option<T> option, T fallback)
    {
        return option.Match(x => x, () => fallback);
    }

    /// <summary>
    /// Asynchronously converts a <see cref="Task{TResult}"/> of <see cref="Option{T}"/> to a <see cref="Task{TResult}"/> of <see cref="Result{T}"/>.
    /// </summary>
    public static Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> optionTask, Error error)
        => optionTask.MatchAsync(x => x, () => Failure<T>(error));
}
