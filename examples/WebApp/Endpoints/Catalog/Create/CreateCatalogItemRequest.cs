using System.ComponentModel.DataAnnotations;
using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Result;
using WebApp.Domain.Catalog;
using WebApp.Services;

namespace WebApp.Endpoints.Catalog.Create;

public class CreateCatalogItemRequest
{
    [MaxLength(CatalogItemSchemaConstants.SkuMaxLength)]
    [Required]
    public required string Sku { get; init; }

    [MaxLength(CatalogItemSchemaConstants.NameMaxLenght)]
    [Required]
    public required string Name { get; init; }

    [MaxLength(CatalogItemSchemaConstants.DescriptionMaxLength)]
    public required string? Description { get; init; }

    [Range(CatalogItemSchemaConstants.PriceMinValue, CatalogItemSchemaConstants.PriceMaxValue)]
    public required decimal Price { get; init; }

    [Range(CatalogItemSchemaConstants.AvailableMinValue, CatalogItemSchemaConstants.AvailableMaxValue)]
    public required decimal Available { get; init; }

    public Result<CatalogService.CreateCatalogItemCommand> ToCommand()
    {
        return
            from sku in Domain.Catalog.Values.Sku.Create(Sku)
            select new CatalogService.CreateCatalogItemCommand(
                sku,
                Name,
                Description ?? string.Empty,
                Price,
                Available);
    }
}
