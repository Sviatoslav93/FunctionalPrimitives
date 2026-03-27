using FunctionalPrimitives.Monads.Results.Extensions;
using WebApp.Domain.Catalog.Values;
using WebApp.Extensions.Http;
using WebApp.Services;

namespace WebApp.Endpoints.Catalog.Publish;

public static class PublishCatalogItemEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("catalog/{sku}/publish", Handle)
            .WithName("publish-item")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static Task<IResult> Handle(
        string sku,
        CatalogService catalogService,
        CancellationToken cancellationToken)
    {
        return Sku.Create(sku)
            .BindAsync(s => catalogService.PublishCatalogItem(s, cancellationToken))
            .ToHttpResultAsync();
    }
}
