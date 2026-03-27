using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Monads.Results;

namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    extension<T>(T? obj) where T : class
    {
        /// <summary>
        /// Converts this nullable reference to a <see cref="Option{T}"/>.
        /// Returns <see cref="Option{T}"/> with a value if non-null; otherwise returns <see cref="Option{T}"/> with no value.
        /// </summary>
        /// <returns>A <see cref="Option{T}"/> containing the value, or an empty <see cref="Option{T}"/> if null.</returns>
        public Option<T> ToMaybe() => obj is null ? None<T>() : Some(obj);

    }

    extension<T>(T? value) where T : struct
    {
        /// <summary>
        /// Converts this nullable value type to a <see cref="Option{T}"/>.
        /// Returns <see cref="Option{T}"/> with a value if it has one; otherwise returns an empty <see cref="Option{T}"/>.
        /// </summary>
        /// <returns>A <see cref="Option{T}"/> containing the value, or an empty <see cref="Option{T}"/> if null.</returns>
        public Option<T> ToMaybe() => value.HasValue ? Some(value.Value) : None<T>();
    }

    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Converts this <see cref="Option{T}"/> to a <see cref="Result{T}"/>.
        /// Returns a successful result if a value is present; otherwise returns a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error to use when no value is present.</param>
        /// <returns>
        /// A successful <see cref="Result{T}"/> if <see cref="Option{T}.HasValue"/> is <see langword="true"/>;
        /// otherwise a failed result containing <paramref name="error"/>.
        /// </returns>
        public Result<T> ToResult(Error error) => option.Match(x => x, () => Failure<T>(error));

        /// <summary>
        /// Returns the contained value if present; otherwise returns the specified fallback value.
        /// </summary>
        /// <param name="fallback">The value to return when no value is present.</param>
        /// <returns>The contained value, or <paramref name="fallback"/> when <see cref="Option{T}.HasValue"/> is <see langword="false"/>.</returns>
        public T GetValueOr(T fallback)
        {
            return option.Match(x => x, () => fallback);
        }
    }

    extension<T>(Task<Option<T>> optionTask)
    {
        /// <summary>
        /// Asynchronously converts a <see cref="Task{TResult}"/> of <see cref="Option{T}"/> to a <see cref="Task{TResult}"/> of <see cref="Result{T}"/>.
        /// The result is successful if the option has a value; otherwise it is a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error to use when no value is present.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to a successful <see cref="Result{T}"/> if a value is present;
        /// otherwise a failed result containing <paramref name="error"/>.
        /// </returns>
        public Task<Result<T>> ToResultAsync(Error error) => optionTask.MatchAsync(x => x, () => Failure<T>(error));
    }
}
