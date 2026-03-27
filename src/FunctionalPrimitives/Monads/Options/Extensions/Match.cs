namespace FunctionalPrimitives.Monads.Options.Extensions;

/// <summary>
/// Extension methods for <see cref="Option{T}"/>.
/// </summary>
public static partial class OptionExtensions
{
    /// <summary>
    /// Matches the current state of the <see cref="Option{T}"/> and executes the appropriate function based on whether a value is available.
    /// </summary>
    public static U Match<T, U>(
        this Option<T> option,
        Func<T, U> onSome,
        Func<U> onNone)
    {
        return option.HasValue
            ? onSome(option.Value)
            : onNone();
    }
}
