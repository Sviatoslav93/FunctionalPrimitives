using WebApp.Domain.Catalog;

namespace WebApp.Endpoints.Catalog.Dtos;

public static class CatalogItemMappings
{
    public static CatalogItemDto ToDto(this CatalogItem item)
    {
        return new CatalogItemDto
        {
            Id = item.Id,
            Sku = item.Sku.Value,
            Price = item.Price,
            Description = item.Description,
            Available = item.Available,
            Status = item.Status,
        };
    }
}
