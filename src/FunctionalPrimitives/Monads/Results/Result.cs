using System.Text.Json;
using System.Text.Json.Serialization;
using FunctionalPrimitives.Abstractions;
using FunctionalPrimitives.Errors;

namespace FunctionalPrimitives.Monads.Results;

/// <summary>
/// Represents the result of an operation, indicating success or failure.
/// </summary>
[JsonConverter(typeof(ResultJsonConverterFactory))]
public sealed class Result<T> : IResult
{
    /// <summary>
    /// The value of the result if it is successful.
    /// </summary>
    private readonly T? _value;

    private readonly Error[]? _errors;

    /// <summary>
    /// Initializes a new instance of the FunctionalPrimitives class with the specified value.
    /// </summary>
    /// <param name="value">The value to be encapsulated by the FunctionalPrimitives instance.</param>
    public Result(T value)
    {
        _value = value;
        _errors = null;
    }

    /// <summary>
    /// Initializes a new instance of the FunctionalPrimitives class with the specified collection of errors.
    /// </summary>
    /// <param name="errors">An array of Error objects representing the errors associated with the result. Cannot be null.</param>
    public Result(Error[] errors)
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
    public IReadOnlyList<Error> Errors => _errors ?? [];

    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome.
    /// </summary>
    public bool IsSuccess => _errors is null;

    /// <summary>
    /// Gets a value indicating whether the result represents a failed outcome.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the value contained in the result if the operation was successful.
    /// </summary>
    /// <remarks>Accessing this property when the result is not successful will throw an exception. Check
    /// <see cref="IsSuccess"/> before accessing <see cref="Value"/> to avoid exceptions.</remarks>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value when Result is a failure.");

    /// <summary>
    /// Gets the collection of errors contained in the result.
    /// Is used only for optimization purposes.
    /// </summary>
    internal Error[] ErrorsInternal => _errors ?? [];

    /// <summary>
    /// Allows implicit conversion from a value of type <typeparamref name="T"/> to a <see cref="Result{T}"/> object.
    /// </summary>
    /// <param name="value">The value to encapsulate in the <see cref="Result{T}"/> instance.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the specified value.</returns>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    /// Defines an implicit conversion from an <see cref="Error"/> instance to a <see cref="Result{T}"/> instance.
    /// </summary>
    /// <param name="error">The error that represents a failed result.</param>
    /// <returns>A new <see cref="Result{T}"/> instance containing the specified error.</returns>
    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>([error]);
    }

    /// <summary>
    /// Defines an implicit conversion from an <see cref="Error"/> instance to a <see cref="Result{T}"/> instance.
    /// </summary>
    /// <param name="error">The error that represents a failed result.</param>
    /// <returns>A new <see cref="Result{T}"/> instance containing the specified error.</returns>
    public static implicit operator Result<T>(Error[] error)
    {
        return new Result<T>(error);
    }

    /// <summary>
    /// Deconstructs the result into its value and collection of errors.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result represents a successful operation.</param>
    /// <param name="result">A tuple containing the value (or <see langword="default"/> on failure) and the array of errors.</param>
    public void Deconstruct(out bool isSuccess, out (T? Value, Error[] Errors) result)
    {
        isSuccess = IsSuccess;
        result = (_value, ErrorsInternal);
    }

    /// <summary>
    /// Returns a string representation of the result, indicating success with the value
    /// or failure with the list of errors.
    /// </summary>
    /// <returns>
    /// <c>"Success(&lt;value&gt;)"</c> when successful, or <c>"Failure(&lt;errors&gt;)"</c> when failed.
    /// </returns>
    public override string ToString()
    {
        return IsSuccess
            ? $"Success({_value})"
            : $"Failure({string.Join(", ", Errors)})";
    }

    /// <summary>
    /// Serializes the result to a JSON string.
    /// </summary>
    /// <returns>A JSON representation of the result, including <c>isSuccess</c> and either <c>value</c> or <c>errors</c>.</returns>
    public string ToJson() => JsonSerializer.Serialize(this);
}
