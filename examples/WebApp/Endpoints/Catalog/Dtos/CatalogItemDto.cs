using WebApp.Domain.Catalog;

namespace WebApp.Endpoints.Catalog.Dtos;

public class CatalogItemDto
{
    public long Id { get; set; }

    public required string Sku { get; init; }

    public required decimal Price { get; init; }

    public required string? Description { get; init; }

    public required decimal Available { get; init; }

    public required ItemStatus Status { get; init; }
}
