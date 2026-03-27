namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Asynchronously executes the specified synchronous action on the contained value of the awaited
    /// <see cref="Option{T}"/> if a value is present, then returns the original <see cref="Option{T}"/>.
    /// </summary>
    public static Task<Option<T>> TapAsync<T>(this Task<Option<T>> optionTask, Action<T> action)
    {
        return optionTask.MatchAsync(
            value =>
            {
                action(value);
                return Some(value);
            },
            () => optionTask);
    }

    /// <summary>
    /// Executes the specified asynchronous action on the contained value of the awaited
    /// <see cref="Option{T}"/> if a value is present, then returns the original <see cref="Option{T}"/>.
    /// </summary>
    public static Task<Option<T>> Tap<T>(this Task<Option<T>> optionTask, Func<T, Task> action)
    {
        return optionTask.MatchAsync(
            async value =>
            {
                await action(value).ConfigureAwait(false);
                return Some(value);
            },
            () => optionTask);
    }

    /// <summary>
    /// Asynchronously executes the specified action when the awaited <see cref="Option{T}"/> has no value,
    /// then returns the original <see cref="Option{T}"/>.
    /// </summary>
    public static Task<Option<T>> TapNoneAsync<T>(this Task<Option<T>> optionTask, Action action)
    {
        return optionTask.MatchAsync(
            _ => optionTask,
            () =>
            {
                action();
                return optionTask;
            });
    }

    /// <summary>
    /// Asynchronously executes the specified asynchronous action when the awaited <see cref="Option{T}"/> has no value,
    /// then returns the original <see cref="Option{T}"/>.
    /// </summary>
    public static Task<Option<T>> TapNoneAsync<T>(this Task<Option<T>> optionTask, Func<Task> action)
    {
        return optionTask.MatchAsync(
            _ => optionTask,
            async () =>
            {
                await action().ConfigureAwait(false);
                return await optionTask;
            });
    }
}
