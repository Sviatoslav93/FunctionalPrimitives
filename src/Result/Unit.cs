using Result.Extensions;

namespace Result;

/// <summary>
/// Represents a type that has no value.
/// </summary>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    private static readonly Unit InitialValue = default;

    public static ref readonly Unit Value => ref InitialValue;

    public static Result<Unit> Result => InitialValue.AsResult();

    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(InitialValue);

    public static bool operator ==(Unit first, Unit second) => true;

    public static bool operator !=(Unit first, Unit second) => false;

    public int CompareTo(Unit other) => 0;

    int IComparable.CompareTo(object? obj) => 0;

    public override int GetHashCode() => 0;

    public bool Equals(Unit other) => true;

    public override bool Equals(object? obj) => obj is Unit;

    public override string ToString() => "()";
}
