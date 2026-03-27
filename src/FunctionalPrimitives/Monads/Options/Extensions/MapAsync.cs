namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Asynchronously transforms the value inside the awaited <see cref="Option{T}"/> when a value is present.
    /// </summary>
    public static Task<Option<U>> MapAsync<T, U>(this Task<Option<T>> optionTask, Func<T, U> projection)
    {
        return optionTask.MatchAsync(
            x => Some(projection(x)),
            None<U>);
    }

    /// <summary>
    /// Asynchronously transforms the value inside the awaited <see cref="Option{T}"/> using an asynchronous projection function when a value is present.
    /// </summary>
    public static Task<Option<U>> MapAsync<T, U>(this Task<Option<T>> optionTask, Func<T, Task<U>> projection)
    {
        return optionTask.MatchAsync(
            async x => Some(await projection(x).ConfigureAwait(false)),
            None<U>);
    }

    /// <summary>
    /// Projects the value inside the awaited <see cref="Option{T}"/> using LINQ query syntax.
    /// </summary>
    public static Task<Option<U>> Select<T, U>(this Task<Option<T>> optionTask, Func<T, U> mapper)
        => optionTask.MapAsync(mapper);

    /// <summary>
    /// Projects the value inside the awaited <see cref="Option{T}"/> using an asynchronous projection function and LINQ query syntax.
    /// </summary>
    public static Task<Option<U>> Select<T, U>(this Task<Option<T>> optionTask, Func<T, Task<U>> mapper)
        => optionTask.MapAsync(mapper);
}
