using Microsoft.EntityFrameworkCore;
using URLShortener.Domain.Entities;

namespace URLShortener.Persistence.Data;

public class UrlDbContext(DbContextOptions<UrlDbContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrl { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}