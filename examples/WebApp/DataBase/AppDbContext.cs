using Microsoft.EntityFrameworkCore;
using WebApp.DataBase.Configuration.Converters;
using WebApp.Domain.Catalog;
using WebApp.Domain.Catalog.Values;
using WebApp.Services.Abstractions;

namespace WebApp.DataBase;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserService? _currentUserService;

    // used for migration generation
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<CatalogItem> CatalogItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<CatalogItem>().HasQueryFilter(i => i.CreatedBy == _currentUserService!.UserId);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder
            .Properties<Sku>()
            .HaveConversion<SkuConverter>()
            .HaveMaxLength(CatalogItemSchemaConstants.SkuMaxLength);
    }
}
