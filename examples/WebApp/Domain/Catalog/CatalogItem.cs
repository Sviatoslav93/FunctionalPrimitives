using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Maybe;
using FunctionalPrimitives.Extensions.Result;
using WebApp.Domain.Base;
using WebApp.Domain.Catalog.Errors;
using WebApp.Domain.Catalog.Values;

namespace WebApp.Domain.Catalog;

public class CatalogItem : AuditableEntity<long>
{
    private CatalogItem(
        Sku sku,
        string name,
        string description,
        decimal price,
        decimal available)
    {
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        Available = available;
        Status = ItemStatus.Inactive;
    }

    public Sku Sku { get; private set; }

    public decimal Price { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public decimal Available { get; private set; }

    public ItemStatus Status { get; private set; }

    public static Result<CatalogItem> Create(
        Sku sku,
        string name,
        string description,
        decimal price,
        decimal available)
    {
        return from n in name.Ensure(x => !string.IsNullOrWhiteSpace(x), CatalogErrors.NameCannotBeEmpty())
            from p in ValidatePrice(price)
            from a in ValidateAvailable(available)
                .Ensure(a => a >= 0, CatalogErrors.AvailableMustBePositive())
            select new CatalogItem(sku, n, description, p, a);
    }

    public Result<Unit> Edit(
        decimal price,
        Maybe<decimal> available,
        Maybe<string> description)
    {
        return from newPrice in ValidatePrice(price)
            from newAvailable in available.Match(ValidateAvailable, () => Success(Available))
            select ApplyEdit(newPrice, newAvailable);

        Unit ApplyEdit(decimal newPrice, decimal newAvailable)
        {
            Price = newPrice;
            Available = newAvailable;
            Description = description.GetValueOr(Description);

            return Unit.Value;
        }
    }

    public Result<Unit> Publish()
    {
        return Status
            .Ensure(x => x == ItemStatus.Inactive, CatalogErrors.ItemCanNotBePublished())
            .Tap(_ => Status = ItemStatus.Active)
            .Ignore();
    }

    public Result<Unit> Remove()
    {
       Status = ItemStatus.Removed;

       return Unit.Value;
    }

    private static Result<decimal> ValidatePrice(decimal price)
    {
        return price
            .Ensure(p => p > CatalogItemSchemaConstants.PriceMinValue, CatalogErrors.PriceMustBePositive())
            .Ensure(p => p <= CatalogItemSchemaConstants.PriceMaxValue, CatalogErrors.PriceTooLarge());
    }

    private static Result<decimal> ValidateAvailable(decimal available)
    {
        return available
            .Ensure(a => a >= 0, CatalogErrors.AvailableMustBePositive());
    }
}
