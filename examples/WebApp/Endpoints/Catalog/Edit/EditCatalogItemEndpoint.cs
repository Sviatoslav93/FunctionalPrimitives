using FunctionalPrimitives.Extensions.Result;
using Microsoft.AspNetCore.Mvc;
using WebApp.Domain.Catalog.Errors;
using WebApp.Extensions;
using WebApp.Extensions.Http;
using WebApp.Services;

namespace WebApp.Endpoints.Catalog.Edit;

public static class EditCatalogItemEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPut("/{sku}", Handle)
            .WithName("edit-item")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static Task<IResult> Handle(
        string sku,
        [FromBody] EditCatalogItemRequest request,
        CatalogService catalogService,
        CancellationToken cancellationToken)
    {
        return request
            .Ensure(r => r.Sku == sku, CatalogErrors.SkuMismatch())
            .Bind(r => r.ToCommand())
            .BindAsync(cmd => catalogService.EditCatalogItem(cmd, cancellationToken))
            .ToHttpResultAsync();
    }
}
