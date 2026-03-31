using Microsoft.Extensions.DependencyInjection;
using MockApi.Infrastructure.StorageProviders.Abstractions;
using MockApi.Infrastructure.StorageProviders.Implementations;

namespace MockApi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageProvider, JsonStorageProvider>();;

        return services;
    }
}