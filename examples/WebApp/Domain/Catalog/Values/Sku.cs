using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Result;
using WebApp.Domain.Catalog.Errors;

namespace WebApp.Domain.Catalog.Values;

public readonly record struct Sku
{
    private Sku(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Sku> Create(string value)
    {
        return value.Ensure(x => !string.IsNullOrWhiteSpace(x), CatalogErrors.SkuEmpty())
            .Map(x => x)
            .Ensure(x => x.Length <= CatalogItemSchemaConstants.SkuMaxLength, CatalogErrors.SkuTooLong())
            .Bind(x => new Sku(x));
    }

    public static Sku ParseOrThrow(string value)
    {
        var result = Create(value);

        return result.Match(
            x => x,
            errors => throw new InvalidOperationException($"Invalid SKU: {string.Join(", ", errors)}"));
    }
}
