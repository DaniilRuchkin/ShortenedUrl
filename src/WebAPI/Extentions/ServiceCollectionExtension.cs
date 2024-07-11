using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Web.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

        services.AddDbContext<UrlDbContext>(options =>
            options.UseMongoDB(connectionString!, databaseName!));

        var host = Environment.GetEnvironmentVariable("REDIS_HOST");
        var key = Environment.GetEnvironmentVariable("REDIS_INSTANCENAME");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = host;
            options.InstanceName = key;
        });

        return services;
    }
}