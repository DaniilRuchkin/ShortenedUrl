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

        return services;
    }
}