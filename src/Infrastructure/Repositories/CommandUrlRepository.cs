using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CommandUrlRepository(UrlDbContext dbContext) : ICommandUrlRepository
{
    public async Task CreateUrlAsync(Entity entityUrl, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(entityUrl, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUrlAsync(Entity entityUrl, CancellationToken cancellationToken)
    {
        dbContext.ShortUrl.Remove(entityUrl);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}