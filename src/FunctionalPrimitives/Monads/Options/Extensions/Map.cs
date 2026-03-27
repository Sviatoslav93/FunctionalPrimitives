namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Transforms the value inside the <see cref="Option{T}"/> when a value is present.
    /// </summary>
    public static Option<U> Map<T, U>(this Option<T> option, Func<T, U> projection)
    {
        return option.Match(
            x => Some(projection(x)),
            None<U>);
    }

    /// <summary>
    /// Projects the value inside the <see cref="Option{T}"/> using LINQ query syntax.
    /// </summary>
    public static Option<U> Select<T, U>(this Option<T> option, Func<T, U> projection)
    {
        return option.Map(projection);
    }
}
