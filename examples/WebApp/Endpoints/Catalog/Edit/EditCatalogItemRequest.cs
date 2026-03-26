using System.ComponentModel.DataAnnotations;
using FunctionalPrimitives.Monads.Results;
using FunctionalPrimitives.Monads.Results.Extensions;
using WebApp.Domain.Catalog;
using WebApp.Services;

namespace WebApp.Endpoints.Catalog.Edit;

public class EditCatalogItemRequest
{
    [MaxLength(CatalogItemSchemaConstants.SkuMaxLength)]
    [Required]
    public required string Sku { get; init; }

    [MaxLength(CatalogItemSchemaConstants.NameMaxLenght)]
    [Required]
    public string? Name { get; init; }

    [MaxLength(CatalogItemSchemaConstants.DescriptionMaxLength)]
    public string? Description { get; init; }

    [Range(CatalogItemSchemaConstants.PriceMinValue, CatalogItemSchemaConstants.PriceMaxValue)]
    public decimal Price { get; init; }

    [Range(CatalogItemSchemaConstants.AvailableMinValue, CatalogItemSchemaConstants.AvailableMaxValue)]
    public decimal Available { get; init; }

    public Result<CatalogService.EditCatalogItemCommand> ToCommand()
    {
        return from sku in Domain.Catalog.Values.Sku.Create(Sku)
               select new CatalogService.EditCatalogItemCommand(
                   sku,
                   Price,
                   Available,
                   Description);
    }
}
