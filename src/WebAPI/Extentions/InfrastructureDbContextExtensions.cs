using Microsoft.EntityFrameworkCore;
using URLShortener.Persistence.Data;

namespace URLShortener.Web.Extentions;

public static class InfrastructureDbContextExtensions
{
    public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

        services.AddDbContext<UrlDbContext>(options =>
            options.UseMongoDB(connectionString!, databaseName!));

        return services;
    }
}