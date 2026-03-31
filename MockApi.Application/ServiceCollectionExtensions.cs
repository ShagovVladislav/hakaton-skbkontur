using Microsoft.Extensions.DependencyInjection;
using MockApi.Application.Services.Abstractions;
using MockApi.Application.Services.Implementations;
using MockApi.Application.Values.Abstractions;
using MockApi.Application.Values.Implementations;

namespace MockApi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<Random>();
        services.AddHttpClient<IFieldTypeInferenceService, FieldTypeInferenceService>();
        services.AddScoped<IValueGenerator, StringValueGenerator>();
        services.AddScoped<IValueGenerator, BooleanValueGenerator>();
        services.AddScoped<IValueGenerator, DateTimeValueGenerator>();
        services.AddScoped<IValueGenerator, IntegerValueGenerator>();
        services.AddScoped<IValueGenerator, DateTimeValueGenerator>();
        services.AddScoped<IValueGenerator, DecimalValueGenerator>();
        services.AddScoped<IValueGenerator, FloatValueGenerator>();
        services.AddScoped<IValueGenerator, GuidValueGenerator>();
        
        services.AddScoped<IMockService, MockService>();

        return services;
    }
}