using WebApp.Extensions.Http;
using WebApp.Services;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApp.Endpoints.Catalog.Lookup;

public static class LookupCatalogItemEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}", Handle)
            .WithName("lookup-item")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static Task<IResult> Handle(
        long id,
        CatalogService catalogService,
        CancellationToken cancellationToken)
    {
        return catalogService
            .LookupCatalogItem(id, cancellationToken)
            .ToHttpResultAsync();
    }
}
