using Microsoft.AspNetCore.Mvc;
using WebApp.Endpoints.Catalog.Dtos;
using WebApp.Services;
using WebApp.Shared;

namespace WebApp.Endpoints.Catalog.Search;

public static class SearchCatalogItemsEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/search", Handle)
            .WithName("search-items")
            .Produces<PagedResponse<CatalogItemDto>>()
            .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    private static async Task<PagedResponse<CatalogItemDto>> Handle(
        [FromBody] SearchCatalogItemsRequest request,
        CatalogService catalogService,
        CancellationToken cancellationToken)
    {
        var query = new CatalogService.SearchCatalogItemQuery(
            request.Page,
            request.PageSize,
            request.OrderBy,
            request.OrderDescending,
            request.Search,
            request.MinPrice,
            request.MaxPrice,
            request.Available,
            request.Statuses);

        var items = await catalogService.SearchCatalogItems(query, cancellationToken);

        return items;
    }
}
