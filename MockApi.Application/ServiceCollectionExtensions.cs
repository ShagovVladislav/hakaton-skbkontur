using Microsoft.Extensions.DependencyInjection;
using MockApi.Application.Services.Abstractions;
using MockApi.Application.Services.Implementations;
using MockApi.Application.Values.Abstractions;
using MockApi.Application.Values.Implementations;

namespace MockApi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<Random>();
        services.AddScoped<IGenerationalValue, StringGenerationalValue>();
        services.AddScoped<IGenerationalValue, BooleanGenerationalValue>();
        services.AddScoped<IGenerationalValue, DateTimeGenerationalValue>();
        services.AddScoped<IGenerationalValue, FirstNameGenerationalValue>();
        services.AddScoped<IGenerationalValue, IntegerGenerationalValue>();
        services.AddScoped<IGenerationalValue, DateGenerationalValue>();

        
        services.AddScoped<IMockService, MockService>();

        return services;
    }
}