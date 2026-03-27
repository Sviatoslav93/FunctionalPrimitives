namespace FunctionalPrimitives;

/// <summary>
/// Represents a type that signifies the absence of a value.
/// This struct is often used in functional programming as a placeholder or a marker type
/// when a method needs to return a result, but the result itself carries no data.
/// </summary>
/// <remarks>
/// The <see cref="Unit"/> type guarantees that all instances are effectively equal, as
/// it does not contain any additional fields or state. It is commonly employed
/// in operations where the return type is only needed to fulfill type constraints or
/// to represent the completion of a task.
/// </remarks>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    /// <summary>
    /// Represents the default initial value of the <see cref="Unit"/> type.
    /// </summary>
    /// <remarks>
    /// This variable is a static readonly instance of <see cref="Unit"/> and is used
    /// internally to ensure consistency when representing an absence of value.
    /// </remarks>
    private static readonly Unit InitialValue = default;

    /// <summary>
    /// Gets a reference to the default instance of the <see cref="Unit"/> type.
    /// </summary>
    /// <remarks>
    /// This property returns a reference to a static readonly instance of <see cref="Unit"/>,
    /// used to represent the absence of a value in a consistent and type-safe manner.
    /// </remarks>
    public static ref readonly Unit Value => ref InitialValue;

    /// <summary>
    /// Gets provide a pre-completed <see cref="Task{Unit}"/> representing an already
    /// completed operation with a result of type <see cref="Unit"/>.
    /// </summary>
    /// <remarks>
    /// This property simplifies scenarios where a completed <see cref="Task{Unit}"/> is
    /// required, eliminating the need to explicitly create a new task instance. It is
    /// commonly used in functional programming constructs or asynchronous flows when
    /// the result type carries no additional data.
    /// </remarks>
    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(InitialValue);

    /// <summary>
    /// Implements the equality operator for comparing two <see cref="Unit"/> instances.
    /// </summary>
    /// <param name="first">The first <see cref="Unit"/> instance to compare.</param>
    /// <param name="second">The second <see cref="Unit"/> instance to compare.</param>
    /// <returns>Always returns true, as all instances of <see cref="Unit"/> are considered equal.</returns>
    public static bool operator ==(Unit first, Unit second) => true;

    /// <summary>
    /// Implements the inequality operator for comparing two <see cref="Unit"/> instances.
    /// </summary>
    /// <param name="first">The first <see cref="Unit"/> instance to compare.</param>
    /// <param name="second">The second <see cref="Unit"/> instance to compare.</param>
    /// <returns>Always returns <see langword="false"/>, as all instances of <see cref="Unit"/> are considered equal.</returns>
    public static bool operator !=(Unit first, Unit second) => false;

    /// <summary>
    /// Compares the current instance with another <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="Unit"/> instance to compare with.</param>
    /// <returns>Always returns <c>0</c>, as all <see cref="Unit"/> instances are considered equal.</returns>
    public int CompareTo(Unit other) => 0;

    /// <summary>
    /// Compares the current instance with another object.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>Always returns <c>0</c>, as all <see cref="Unit"/> instances are considered equal.</returns>
    int IComparable.CompareTo(object? obj) => 0;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>Always returns <c>0</c>, as all <see cref="Unit"/> instances are equivalent.</returns>
    public override int GetHashCode() => 0;

    /// <summary>
    /// Indicates whether this instance is equal to another <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="Unit"/> instance to compare with.</param>
    /// <returns>Always returns <see langword="true"/>, as all <see cref="Unit"/> instances are considered equal.</returns>
    public bool Equals(Unit other) => true;

    /// <summary>
    /// Indicates whether this instance is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="Unit"/> instance; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is Unit;

    /// <summary>
    /// Returns the string representation of this instance.
    /// </summary>
    /// <returns>The string <c>"()"</c>, which is the conventional notation for the unit type.</returns>
    public override string ToString() => "()";
}
