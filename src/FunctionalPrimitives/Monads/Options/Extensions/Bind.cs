namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Applies a function to the value contained in the <see cref="Option{T}"/> if a value is present.
    /// </summary>
    public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> binder)
    {
        return option.HasValue
            ? binder(option.Value)
            : None<U>();
    }

    /// <summary>
    /// Projects the value contained in the <see cref="Option{T}"/> to another <see cref="Option{T}"/> using a specified transformation function.
    /// </summary>
    public static Option<U> SelectMany<T, U>(this Option<T> option, Func<T, Option<U>> binder)
    {
        return option.Bind(binder);
    }

    /// <summary>
    /// Projects each element of the <see cref="Option{T}"/> into a new form using a binding function
    /// and applies a result selector function to produce a final result.
    /// </summary>
    public static Option<U> SelectMany<T, V, U>(
        this Option<T> option,
        Func<T, Option<V>> binder,
        Func<T, V, U> projector)
    {
        return option.Bind(t =>
            binder(t).Map(i => projector(t, i)));
    }
}
