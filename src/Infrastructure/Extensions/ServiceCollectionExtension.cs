using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        var host = Environment.GetEnvironmentVariable("REDIS_HOST");
        var key = Environment.GetEnvironmentVariable("REDIS_INSTANCENAME");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = host;
            options.InstanceName = key;
        });

        services.AddScoped<IRedisCacheService, RedisCacheService>();

        return services;
    }
}