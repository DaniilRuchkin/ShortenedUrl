using Application.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Extensions;

public static class InfrastructureRedisCacheExtensions
{
    public static IServiceCollection AddInfrastructureRedisCacheServices(this IServiceCollection services)
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