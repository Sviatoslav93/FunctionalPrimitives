using FunctionalPrimitives;
using FunctionalPrimitives.Extensions.Maybe;
using FunctionalPrimitives.Extensions.Result;
using Microsoft.EntityFrameworkCore;
using WebApp.DataBase;
using WebApp.Domain.Catalog;
using WebApp.Domain.Catalog.Errors;
using WebApp.Domain.Catalog.Values;
using WebApp.Endpoints.Catalog.Dtos;
using WebApp.Shared;
using WebApp.Shared.Extensions;

namespace WebApp.Services;

public class CatalogService(
    AppDbContext db)
{
    public async Task<Result<CatalogItemDto>> LookupCatalogItem(long id, CancellationToken cancellationToken = default)
    {
        var item = await db.CatalogItems
            .AsQueryable()
            .Where(i => i.Id == id)
            .Select(i => new CatalogItemDto()
            {
                Sku = i.Sku.Value,
                Price = i.Price,
                Description = i.Description,
                Available = i.Available,
                Status = i.Status,
            })
            .FirstOrDefaultAsync(cancellationToken);

        return item.ToResult(CatalogErrors.CatalogItemNotFound(id));
    }

    public async Task<PagedResponse<CatalogItem>> SearchCatalogItems(
        SearchCatalogItemQuery request,
        CancellationToken cancellationToken = default)
    {
        var (search, minPrice, maxPrice, available, statuses)
            = (request.Search, request.MinPrice, request.MaxPrice, request.Available, request.Statuses);

        var query = db.CatalogItems.AsQueryable();

        if (search is not null)
        {
            query = query.Where(i => i.Sku.Value.Contains(search) || i.Description.Contains(search));
        }

        if (minPrice is not null)
        {
            query = query.Where(i => i.Price >= minPrice);
        }

        if (maxPrice is not null)
        {
            query = query.Where(i => i.Price <= maxPrice);
        }

        if (available is not null)
        {
            query = query.Where(i => i.Available <= available);
        }

        if (statuses is not null)
        {
            query = query.Where(i => statuses.Any(s => s == i.Status));
        }

        var items = await query
            .ApplySorting(typeof(CatalogItem), request)
            .ApplyPaging(request)
            .ToArrayAsync(cancellationToken);

        var count = await db.CatalogItems.CountAsync(cancellationToken);

        return new PagedResponse<CatalogItem>
        {
            Count = count,
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }

    public async Task<Result<Sku>> CreateCatalogItem(
        CreateCatalogItemCommand cmd,
        CancellationToken cancellationToken = default)
    {
        var (isSuccess, catalogItemNotExist) = await CheckCatalogItemNotExists(cmd.Sku, cancellationToken);
        if (!isSuccess)
        {
            return catalogItemNotExist.Errors;
        }

        return await CheckCatalogItemNotExists(cmd.Sku, cancellationToken)
            .BindAsync(_ => AddCatalogItem())
            .TapAsync(_ => db.SaveChangesAsync(cancellationToken))
            .MapAsync(i => i.Sku);

        Result<CatalogItem> AddCatalogItem()
        {
            return CatalogItem.Create(cmd.Sku, cmd.Name, cmd.Description, cmd.Price, cmd.Available)
                .Tap(item => db.CatalogItems.Add(item));
        }
    }

    public Task<Result<Unit>> EditCatalogItem(EditCatalogItemCommand cmd, CancellationToken cancellationToken = default)
    {
        return LoadCatalogItem(cmd.Sku, cancellationToken)
            .BindAsync(item => item.Edit(cmd.Price, cmd.Available.ToMaybe(), cmd.Description.ToMaybe()))
            .TapAsync(_ => db.SaveChangesAsync(cancellationToken))
            .IgnoreAsync();
    }

    public Task<Result<Unit>> PublishCatalogItem(Sku sku, CancellationToken cancellationToken = default)
    {
        return LoadCatalogItem(sku, cancellationToken)
            .TapAsync(item => item.Publish())
            .TapAsync(_ => db.SaveChangesAsync(cancellationToken))
            .IgnoreAsync();
    }

    public Task<Result<Unit>> RemoveCatalogItem(Sku sku, CancellationToken cancellationToken = default)
    {
        return LoadCatalogItem(sku, cancellationToken)
            .TapAsync(item => item.Remove())
            .TapAsync(_ => db.SaveChangesAsync(cancellationToken))
            .IgnoreAsync();
    }

    // load catalog item aggregate from db
    private async Task<Result<CatalogItem>> LoadCatalogItem(Sku sku, CancellationToken cancellationToken = default)
    {
        var item = await db.CatalogItems.FirstOrDefaultAsync(i => i.Sku == sku, cancellationToken);

        return item.ToResult(new Error("catalog item not found", "catalog-item-not-found"));
    }

    private async Task<Result<Unit>> CheckCatalogItemNotExists(Sku sku, CancellationToken cancellationToken = default)
    {
        var exist = await db.CatalogItems.AnyAsync(i => i.Sku == sku, cancellationToken);

        return exist ? Failure<Unit>(CatalogErrors.ItemAlreadyExists(sku.Value)) : Unit.Value;
    }

    public record SearchCatalogItemQuery(
        int Page,
        int PageSize,
        string? OrderBy,
        bool OrderDescending,
        string? Search,
        decimal? MinPrice,
        decimal? MaxPrice,
        decimal? Available,
        ItemStatus[]? Statuses)
        : IPaginationRequest, IOrderingRequest;

    public record CreateCatalogItemCommand(
        Sku Sku,
        string Name,
        string Description,
        decimal Price,
        decimal Available);

    public record EditCatalogItemCommand(
        Sku Sku,
        decimal Price,
        decimal? Available = null,
        string? Description = null);
}
