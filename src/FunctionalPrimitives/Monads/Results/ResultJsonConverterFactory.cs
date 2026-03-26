using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunctionalPrimitives.Monads.Results;

/// <summary>
/// A <see cref="JsonConverterFactory"/> that creates <see cref="ResultJsonConverter{T}"/> instances
/// for any closed generic <see cref="Result{T}"/> type, enabling automatic JSON serialization support.
/// </summary>
public sealed class ResultJsonConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// Determines whether this factory can convert the specified type.
    /// Returns <see langword="true"/> only for closed generic <see cref="Result{T}"/> types.
    /// </summary>
    /// <param name="typeToConvert">The type to check.</param>
    /// <returns><see langword="true"/> if the type is a <see cref="Result{T}"/>; otherwise <see langword="false"/>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);
    }

    /// <summary>
    /// Creates a <see cref="ResultJsonConverter{T}"/> for the given <see cref="Result{T}"/> type.
    /// </summary>
    /// <param name="type">The closed generic <see cref="Result{T}"/> type to create a converter for.</param>
    /// <param name="options">The serializer options (not used directly).</param>
    /// <returns>A <see cref="JsonConverter"/> for the specified <see cref="Result{T}"/> type.</returns>
    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var valueType = type.GetGenericArguments()[0];

        var converterType = typeof(ResultJsonConverter<>).MakeGenericType(valueType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

