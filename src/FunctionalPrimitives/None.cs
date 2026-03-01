namespace FunctionalPrimitives;

/// <summary>
/// Represents the absence of a value and is used as a placeholder when no meaningful value is needed.
/// </summary>
/// <remarks>
/// The <see cref="None"/> struct is used in scenarios where a value of a method, task, or result
/// is intentionally ignored or not required. It ensures type safety by explicitly denoting the absence
/// of a value rather than using null.
/// </remarks>
public readonly struct None : IEquatable<None>, IComparable<None>, IComparable
{
    /// <summary>
    /// Represents the default initial value of the <see cref="None"/> type.
    /// </summary>
    /// <remarks>
    /// This variable is a static readonly instance of <see cref="None"/> and is used
    /// internally to ensure consistency when representing an absence of value.
    /// </remarks>
    private static readonly None InitialValue = default;

    /// <summary>
    /// Gets a reference to the default instance of the <see cref="None"/> type.
    /// </summary>
    /// <remarks>
    /// This property returns a reference to a static readonly instance of <see cref="None"/>,
    /// used to represent the absence of a value in a consistent and type-safe manner.
    /// </remarks>
    public static ref readonly None Value => ref InitialValue;

    /// <summary>
    /// Gets a completed <see cref="System.Threading.Tasks.Task"/> that represents the absence of a value.
    /// </summary>
    /// <remarks>
    /// This property provides a pre-completed task with a result of the <see cref="None"/> type.
    /// It is intended for scenarios where a task needs to be returned but no meaningful value
    /// is required or expected.
    /// </remarks>
    public static Task<None> Task { get; } = System.Threading.Tasks.Task.FromResult(InitialValue);

    /// <summary>
    /// Implements the equality operator for comparing two <see cref="None"/> instances.
    /// </summary>
    /// <param name="first">The first <see cref="None"/> instance to compare.</param>
    /// <param name="second">The second <see cref="None"/> instance to compare.</param>
    /// <returns>Always returns true, as all instances of <see cref="None"/> are considered equal.</returns>
    public static bool operator ==(None first, None second) => true;

    public static bool operator !=(None first, None second) => false;

    public int CompareTo(None other) => 0;

    int IComparable.CompareTo(object? obj) => 0;

    public override int GetHashCode() => 0;

    public bool Equals(None other) => true;

    public override bool Equals(object? obj) => obj is None;

    public override string ToString() => "()";
}
