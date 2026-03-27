using FunctionalPrimitives.Errors;
using FunctionalPrimitives.Errors.Extensions;
using WebApp.Domain.Catalog.Values;

namespace WebApp.Domain.Catalog.Errors;

public static class CatalogErrors
{
    public static Error PriceMustBePositive() =>
        new ValidationError("Price must be positive", "catalog-item.price-must-be-positive");

    public static Error PriceTooLarge() =>
        new ValidationError("Price cannot exceed 1000000", "catalog-item.price-too-large");

    public static Error AvailableMustBePositive() =>
        new ValidationError("Available quantity must be positive", "catalog-item.available-must-be-positive");

    public static Error ItemCanNotBePublished() =>
        new ValidationError("Item can not be published", "catalog-item.can-not-be-published");

    public static Error NameCannotBeEmpty() =>
        new ValidationError("Name cannot be empty", "catalog-item.name-cannot-be-empty");

    public static Error CatalogItemNotFound(Sku sku) =>
        new NotFoundError("Catalog item not found", "catalog-item.not-found")
            .WithMetadata("sku", sku);

    public static Error SkuMismatch() =>
        new ValidationError("SKU mismatch", "catalog-item.sku-mismatch");

    public static Error ItemAlreadyExists(Sku sku) =>
        new ConflictError("Item already exists", "catalog-item.already-exists")
            .WithMetadata("sku", sku);

    public static Error SkuEmpty() =>
        new ValidationError("SKU cannot be empty", "catalog-item.sku-empty");

    public static Error SkuTooLong() =>
        new ValidationError("SKU cannot be longer than 32 characters", "catalog-item.sku-too-long");

    public static Error ItemNotFound(Sku sku) =>
        new NotFoundError("Catalog item not found", "catalog-item.not-found")
            .WithMetadata("sku", sku);
}
