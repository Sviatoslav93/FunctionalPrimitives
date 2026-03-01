namespace FunctionalPrimitives;

/// <summary>
/// Represents the result of an operation, indicating success or failure.
/// </summary>
public sealed class Result<T> : IResult
{
    /// <summary>
    /// The value of the result if it is successful.
    /// </summary>
    private readonly T? _value;

    private readonly Error[] _errors;

    /// <summary>
    /// Initializes a new instance of the FunctionalPrimitives class with the specified value.
    /// </summary>
    /// <param name="value">The value to be encapsulated by the FunctionalPrimitives instance.</param>
    private Result(T value)
    {
        _value = value;
        _errors = [];
    }

    /// <summary>
    /// Initializes a new instance of the FunctionalPrimitives class with the specified collection of errors.
    /// </summary>
    /// <param name="errors">An array of Error objects representing the errors associated with the result. Cannot be null.</param>
    private Result(params Error[] errors)
    {
        if (errors.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        _value = default;
        _errors = [.. errors];
    }

    /// <summary>
    /// Gets the collection of errors encountered during the operation.
    /// </summary>
    public IReadOnlyCollection<Error> Errors => _errors;

    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome.
    /// </summary>
    public bool IsSuccess => Errors.Count == 0;

    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the value contained in the result if the operation was successful.
    /// </summary>
    /// <remarks>Accessing this property when the result is not successful will throw an exception. Check
    /// <see cref="IsSuccess"/> before accessing <see cref="Value"/> to avoid exceptions.</remarks>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("FunctionalPrimitives is not successful and value can not be accessed.");

    /// <summary>
    /// Allows implicit conversion from a value of type <typeparamref name="T"/> to a <see cref="FunctionalPrimitives{T}"/> object.
    /// </summary>
    /// <param name="value">The value to encapsulate in the <see cref="FunctionalPrimitives{T}"/> instance.</param>
    /// <returns>A successful <see cref="FunctionalPrimitives{T}"/> containing the specified value.</returns>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    /// Defines an implicit conversion from an <see cref="Error"/> instance to a <see cref="FunctionalPrimitives{T}"/> instance.
    /// </summary>
    /// <param name="error">The error that represents a failed result.</param>
    /// <returns>A new <see cref="FunctionalPrimitives{T}"/> instance containing the specified error.</returns>
    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }

    /// <summary>
    /// Implicitly converts an array of <see cref="Error"/> objects into a <see cref="FunctionalPrimitives{T}"/> instance
    /// representing a failure.
    /// </summary>
    /// <param name="errors">The array of <see cref="Error"/> objects that describe the failure.</param>
    /// <returns>A <see cref="FunctionalPrimitives{T}"/> instance representing a failed result containing the provided errors.</returns>
    public static implicit operator Result<T>(Error[] errors)
    {
        return new Result<T>(errors);
    }

    /// <summary>
    /// Deconstructs the result into its value and collection of errors.
    /// </summary>
    /// <param name="value">The value encapsulated by the result if the operation is successful; otherwise, null.</param>
    /// <param name="errors">The collection of errors associated with the result.</param>
    public void Deconstruct(out T? value, out IReadOnlyCollection<Error> errors)
    {
        value = _value;
        errors = Errors;
    }
}
