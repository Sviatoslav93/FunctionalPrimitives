using Microsoft.EntityFrameworkCore;
using WebApp.Services.Abstractions;

namespace WebApp.DataBase.Seed;

public class CatalogSeeder(ICurrentUserService currentUserService)
{
    public async Task SeedAsync(AppDbContext db)
    {
        Directory.CreateDirectory("Db");

        await db.Database.EnsureCreatedAsync();

        var exists = await db.CatalogItems.AnyAsync();

        if (exists)
            return;

        var userId = currentUserService.UserId;

        await db.Database.ExecuteSqlAsync($"""
            INSERT OR IGNORE INTO CatalogItems
            (Sku, Price, Name, Description, Available, Status, CreatedAt, CreatedBy)
            VALUES
            ('IPHONE-15-128-BLK', 999, 'iPhone 15', 'Apple iPhone 15 128GB Black', 50, 1, CURRENT_TIMESTAMP, {userId}),
            ('IPHONE-15-256-BLK', 1099, 'iPhone 15', 'Apple iPhone 15 256GB Black', 40, 1, CURRENT_TIMESTAMP, {userId}),
            ('IPHONE-15-PRO-128', 1199, 'iPhone 15 Pro', 'Apple iPhone 15 Pro 128GB', 30, 1, CURRENT_TIMESTAMP, {userId}),
            ('IPHONE-15-PRO-256', 1299, 'iPhone 15 Pro', 'Apple iPhone 15 Pro 256GB', 25, 1, CURRENT_TIMESTAMP, {userId}),
            ('IPHONE-15-PM-256', 1399, 'iPhone 15 Pro Max', 'Apple iPhone 15 Pro Max 256GB', 20, 1, CURRENT_TIMESTAMP, {userId});
            """);
    }
}
