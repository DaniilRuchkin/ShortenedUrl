using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjeciton
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICommandUrlRepository, CommandUrlRepository>();
        services.AddScoped<IQueryUrlRepository, QueryUrlRepository>();

        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

        services.AddDbContext<UrlDbContext>(options =>
            options.UseMongoDB(connectionString!, databaseName!));

        return services;
    }
}