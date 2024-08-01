using Microsoft.EntityFrameworkCore;
using URLShortener.Application.Interfaces;
using URLShortener.Infrastructure.Services;
using URLShortener.Persistence.Data;

namespace URLShortener.Web.Extentions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

        services.AddDbContext<UrlDbContext>(options =>
            options.UseMongoDB(connectionString!, databaseName!));

        return services;
    }

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