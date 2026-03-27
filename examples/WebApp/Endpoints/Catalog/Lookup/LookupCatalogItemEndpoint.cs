using FunctionalPrimitives.Monads.Results.Extensions;
using WebApp.Domain.Catalog.Values;
using WebApp.Endpoints.Catalog.Dtos;
using WebApp.Extensions.Http;
using WebApp.Services;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApp.Endpoints.Catalog.Lookup;

public static class LookupCatalogItemEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{sku}", Handle)
            .WithName("lookup-item")
            .Produces<CatalogItemDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static Task<IResult> Handle(
        string sku,
        CatalogService catalogService,
        CancellationToken cancellationToken)
    {
        return Sku.Create(sku)
            .BindAsync(x => catalogService.LookupCatalogItem(x, cancellationToken))
            .ToHttpResultAsync();
    }
}
