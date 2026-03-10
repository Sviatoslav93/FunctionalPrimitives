using FluentValidation;
using WebApp.Domain.Catalog;

namespace WebApp.Endpoints.Catalog.Edit;

public class EditCatalogItemValidator : AbstractValidator<EditCatalogItemRequest>
{
    public EditCatalogItemValidator()
    {
        RuleFor(r => r.Sku)
            .NotEmpty()
            .Length(CatalogItemSchemaConstants.SkuMaxLength);

        RuleFor(r => r.Price)
            .ExclusiveBetween(CatalogItemSchemaConstants.PriceMinValue, CatalogItemSchemaConstants.PriceMaxValue);
    }
}
