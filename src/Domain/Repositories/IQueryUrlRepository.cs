using Domain.Entities;

namespace Domain.Repositories;

public interface IQueryUrlRepository
{
    public Task<Entity> GetUrlAsync(string shortenedUrl, CancellationToken cancellationToken);
}