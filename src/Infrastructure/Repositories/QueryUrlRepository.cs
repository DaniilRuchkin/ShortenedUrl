using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QueryUrlRepository(UrlDbContext dbContext) : IQueryUrlRepository
{
    public async Task<Entity> GetUrlAsync(string shortenedUrl, CancellationToken cancellationToken)
    {
        var getShortenedUrl = await dbContext.ShortUrl.
            AsNoTracking().
            FirstOrDefaultAsync(url => url.ShortenedUrl == shortenedUrl, cancellationToken);

        return getShortenedUrl!;
    }
}