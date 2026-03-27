using FunctionalPrimitives.Monads.Results.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extensions.Http;
using WebApp.Filters;
using WebApp.Services;

namespace WebApp.Endpoints.Catalog.Create;

public static class CreateCatalogItemEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", Handle)
            .AddEndpointFilter<ValidationFilter<CreateCatalogItemRequest>>()
            .WithName("create-item")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
    }

    private static Task<IResult> Handle(
        [FromBody] CreateCatalogItemRequest request,
        CatalogService catalogService,
        CancellationToken cancellationToken)
    {
        return request.ToCommand()
            .BindAsync(cmd => catalogService.CreateCatalogItem(cmd, cancellationToken))
            .ToHttpResultAsync();
    }
}
