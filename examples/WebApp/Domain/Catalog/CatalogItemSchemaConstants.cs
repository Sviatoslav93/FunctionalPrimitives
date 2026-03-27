namespace WebApp.Domain.Catalog;

public static class CatalogItemSchemaConstants
{
    public const int SkuMaxLength = 100;
    public const int NameMaxLenght = 500;
    public const int DescriptionMaxLength = 1000;
    public const int PriceMaxValue = 1_000_000;
    public const int PriceMinValue = 0;
    public const int AvailableMaxValue = 1_000_000;
    public const int AvailableMinValue = 0;
}
