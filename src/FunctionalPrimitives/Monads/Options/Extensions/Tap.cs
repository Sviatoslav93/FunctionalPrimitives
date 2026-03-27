namespace FunctionalPrimitives.Monads.Options.Extensions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Executes the specified action on the contained value if a value is present, then returns the original <see cref="Option{T}"/>.
    /// </summary>
    public static Option<T> Tap<T>(this Option<T> option, Action<T> action)
    {
        return option.Match(
            value =>
            {
                action(value);
                return Some(value);
            },
            () => option);
    }

    /// <summary>
    /// Executes the specified action when no value is present, then returns the original <see cref="Option{T}"/>.
    /// </summary>
    public static Option<T> TapNone<T>(this Option<T> option, Action action)
    {
        return option.Match(
            Some,
            () =>
            {
                action();
                return option;
            });
    }
}
