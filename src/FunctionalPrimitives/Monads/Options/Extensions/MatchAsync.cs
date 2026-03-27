namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Asynchronously matches the state of the awaited <see cref="Option{T}"/> and executes
    /// the appropriate function based on whether a value is present.
    /// </summary>
    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, U> onSome,
        Func<U> onNone)
    {
        return (await optionTask.ConfigureAwait(false)).Match(onSome, onNone);
    }

    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<U>> onSome,
        Func<U> onNone)
    {
        var maybe = await optionTask.ConfigureAwait(false);
        return maybe.HasValue
            ? await onSome(maybe.Value).ConfigureAwait(false)
            : onNone();
    }

    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, U> onSome,
        Func<Task<U>> onNone)
    {
        var maybe = await optionTask.ConfigureAwait(false);
        return maybe.HasValue
            ? onSome(maybe.Value)
            : await onNone().ConfigureAwait(false);
    }

    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<U>> onSome,
        Func<Task<U>> onNone)
    {
        var maybe = await optionTask.ConfigureAwait(false);
        return maybe.HasValue
            ? await onSome(maybe.Value).ConfigureAwait(false)
            : await onNone().ConfigureAwait(false);
    }
}
