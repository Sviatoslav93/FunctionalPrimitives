using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunctionalPrimitives;

/// <summary>
/// A <see cref="JsonConverter{T}"/> for <see cref="Result{T}"/> that serializes the result as a JSON object
/// with an <c>isSuccess</c> flag, plus either a <c>value</c> or an <c>errors</c> array.
/// Deserialization is not supported and will throw <see cref="NotImplementedException"/>.
/// </summary>
/// <typeparam name="T">The type of the value contained in the result when successful.</typeparam>
public sealed class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    /// <summary>
    /// Writes a <see cref="Result{T}"/> as JSON.
    /// Successful results are written with <c>isSuccess: true</c> and a <c>value</c> property;
    /// failed results are written with <c>isSuccess: false</c> and an <c>errors</c> array.
    /// </summary>
    /// <param name="writer">The writer to write JSON to.</param>
    /// <param name="result">The result value to serialize.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    public override void Write(Utf8JsonWriter writer, Result<T> result, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteBoolean("isSuccess", result.IsSuccess);

        if (result.IsSuccess)
        {
            writer.WritePropertyName("value");
            JsonSerializer.Serialize(writer, result.Value, options);
        }
        else
        {
            writer.WritePropertyName("errors");
            JsonSerializer.Serialize(writer, result.Errors, options);
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Reading <see cref="Result{T}"/> from JSON is not supported.
    /// </summary>
    /// <exception cref="NotImplementedException">Always thrown.</exception>
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

