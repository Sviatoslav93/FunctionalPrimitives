using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Errors;
using WebApp.Shared;

namespace WebApp.Domain.Catalog.Errors;

public static class CatalogErrors
{
    public static Error PriceMustBePositive() =>
        Error.FromCode("catalog-item.invalid-price", "Invalid price", ErrorTypes.Validation);

    public static Error PriceTooLarge() =>
        Error.FromCode("catalog-item.price-too-large", "Price cannot exceed 1000000", ErrorTypes.Validation);

    public static Error AvailableMustBePositive() =>
        Error.FromCode("catalog-item.available-must-be-positive", "Available quantity must be positive", ErrorTypes.Validation);

    public static Error ItemCanNotBePublished() =>
        Error.FromCode("catalog-item.can-not-be-published", "Item can not be published", ErrorTypes.Forbidden);

    public static Error NameCannotBeEmpty() =>
        Error.FromCode("catalog-item.name-cannot-be-empty", "Name cannot be empty", ErrorTypes.Validation);

    public static Error CatalogItemNotFound(long id) =>
        Error.FromCode("catalog-item.not-found", "Catalog item not found", ErrorTypes.NotFound)
            .WithMetadata("id", id);

    public static Error SkuMismatch() =>
        Error.FromCode("catalog-item.sku-mismatch", "SKU mismatch", ErrorTypes.Failure);

    public static Error ItemAlreadyExists(string sku) =>
        Error.FromCode("catalog-item.already-exists", "Item already exists", ErrorTypes.Conflict)
            .WithMetadata("sku", sku);

    public static Error SkuEmpty() =>
        Error.FromCode("catalog-item.sku-empty", "SKU cannot be empty", ErrorTypes.Validation);

    public static Error SkuTooLong() =>
        Error.FromCode("catalog-item.sku-too-long", "SKU cannot be longer than 32 characters", ErrorTypes.Validation);
}
