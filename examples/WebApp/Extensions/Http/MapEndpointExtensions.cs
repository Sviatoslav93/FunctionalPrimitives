using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApp.Extensions.Http;

public static class MapEndpointExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoints>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
        }
    }

    public static void RegisterEndpointsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        var assembly = typeof(T).Assembly;

        var endpointTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpoints)) && t is { IsClass: true, IsAbstract: false, IsInterface: false });

        var serviceDescriptors = endpointTypes
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoints), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
    }
}
