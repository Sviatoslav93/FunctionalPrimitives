namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Asynchronously applies a binder function to the value inside the awaited <see cref="Option{T}"/> if a value is present.
    /// </summary>
    public static async Task<Option<U>> BindAsync<T, U>(this Task<Option<T>> optionTask, Func<T, Option<U>> binder)
    {
        var maybe = await optionTask.ConfigureAwait(false);
        return maybe.Bind(binder);
    }

    /// <summary>
    /// Asynchronously applies an asynchronous binder function to the value inside the awaited <see cref="Option{T}"/> if a value is present.
    /// </summary>
    public static async Task<Option<U>> BindAsync<T, U>(this Task<Option<T>> optionTask, Func<T, Task<Option<U>>> binder)
    {
        var maybe = await optionTask.ConfigureAwait(false);

        return maybe.HasValue
            ? await binder(maybe.Value).ConfigureAwait(false)
            : None<U>();
    }

    /// <summary>
    /// Projects the value inside the awaited <see cref="Option{T}"/> to another <see cref="Option{T}"/>
    /// using the specified binder function, enabling LINQ query syntax.
    /// </summary>
    public static Task<Option<U>> SelectMany<T, U>(this Task<Option<T>> optionTask, Func<T, Option<U>> binder)
        => optionTask.BindAsync(binder);

    /// <summary>
    /// Projects each element of the awaited <see cref="Option{T}"/> into a new form using a binder
    /// and applies a result selector to produce the final result, enabling LINQ query syntax.
    /// </summary>
    public static Task<Option<U>> SelectMany<T, V, U>(
        this Task<Option<T>> optionTask,
        Func<T, Option<V>> binder,
        Func<T, V, U> projector)
        => optionTask.BindAsync(t =>
            binder(t).Map(i => projector(t, i)));

    /// <summary>
    /// Projects each element of the awaited <see cref="Option{T}"/> into a new form using an asynchronous binder
    /// and applies a result selector to produce the final result, enabling LINQ query syntax.
    /// </summary>
    public static Task<Option<U>> SelectMany<T, V, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<Option<V>>> binder,
        Func<T, V, U> projector)
        => optionTask.BindAsync(t =>
            binder(t).MapAsync(i => projector(t, i)));
}
