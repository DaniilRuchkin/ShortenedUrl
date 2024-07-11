using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Data;

public class UrlDbContext(DbContextOptions<UrlDbContext> options) : DbContext(options)
{
    public DbSet<Entity> ShortUrl { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entity>();
    }
}