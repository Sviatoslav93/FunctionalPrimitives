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
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var catalogSeeder = scope.ServiceProvider.GetRequiredService<CatalogSeeder>();
await catalogSeeder.SeedAsync(db);

app.MapEndpoints();

app.Run();
