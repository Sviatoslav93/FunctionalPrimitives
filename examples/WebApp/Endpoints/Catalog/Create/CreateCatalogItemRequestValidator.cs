using FluentValidation;
using WebApp.Domain.Catalog;

namespace WebApp.Endpoints.Catalog.Create;

public class CreateCatalogItemRequestValidator : AbstractValidator<CreateCatalogItemRequest>
{
    public CreateCatalogItemRequestValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty();

        RuleFor(x => x.Price)
            .ExclusiveBetween(CatalogItemSchemaConstants.PriceMinValue, CatalogItemSchemaConstants.PriceMaxValue);
    }
}
