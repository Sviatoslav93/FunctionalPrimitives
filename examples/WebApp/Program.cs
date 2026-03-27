using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApp;
using WebApp.DataBase;
using WebApp.DataBase.Seed;
using WebApp.Extensions.Http;
using WebApp.Services;
using WebApp.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddScoped<CatalogService>();
services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton(TimeProvider.System);

services.AddValidatorsFromAssemblyContaining<Program>();

services.AddDbContext<AppDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("StoreDb")));
services.AddScoped<AuditableEntityInterceptor>();
services.AddScoped<CatalogSeeder>();

services.RegisterEndpointsFromAssemblyContaining<IApiMark>();
services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var catalogSeeder = scope.ServiceProvider.GetRequiredService<CatalogSeeder>();
await catalogSeeder.SeedAsync(db);

app.MapEndpoints();

app.Run();
