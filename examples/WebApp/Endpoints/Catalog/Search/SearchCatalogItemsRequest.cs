using System.ComponentModel.DataAnnotations;
using WebApp.Domain.Catalog;
using WebApp.Shared;

namespace WebApp.Endpoints.Catalog.Search;

public class SearchCatalogItemsRequest : IPaginationRequest, IOrderingRequest
{
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 5;

    public string? OrderBy { get; init; }

    public bool OrderDescending { get; init; }

    public string? Search { get; init; }

    // filters
    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public decimal? Available { get; init; }

    public ItemStatus[]? Statuses { get; init; }
}
