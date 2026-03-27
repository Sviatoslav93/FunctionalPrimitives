using WebApp.Endpoints.Catalog.Create;
using WebApp.Endpoints.Catalog.Edit;
using WebApp.Endpoints.Catalog.Lookup;
using WebApp.Endpoints.Catalog.Search;

namespace WebApp.Endpoints.Catalog;

public class CatalogItemsEndpoints : IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/catalog-items")
            ;

        SearchCatalogItemsEndpoint.Map(group);
        LookupCatalogItemEndpoint.Map(group);
        CreateCatalogItemEndpoint.Map(group);
        EditCatalogItemEndpoint.Map(group);
    }
}
