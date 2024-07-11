using Domain.Entities;

namespace Domain.Repositories;

public interface ICommandUrlRepository
{
    public Task CreateUrlAsync(Entity entityUrl, CancellationToken cancellationToken);

    public Task DeleteUrlAsync(Entity entityUrl, CancellationToken cancellationToken);
}