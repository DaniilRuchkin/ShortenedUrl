using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Url.Commands.Delete;

public class DeleteUrlCommandHandler(IPasswordHasher<string> passwordHasher,
    UrlDbContext context, IRedisCacheService cache) : IRequestHandler<DeleteUrlCommand>
{
    public async Task Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = await context.ShortUrl.
            AsNoTracking().
            FirstOrDefaultAsync(url => url.ShortenedUrl == request.shortenedUrl, cancellationToken);

        if (entityToDelete == null)
        {
            throw new NullReferenceException();
        }

        var passwordVerificationPassword = passwordHasher.VerifyHashedPassword(null!, entityToDelete.Password!, request.password);

        if (passwordVerificationPassword != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException();
        }

        context.ShortUrl.Remove(entityToDelete);
        await context.SaveChangesAsync(cancellationToken);

        var chaceKey = $"{entityToDelete.OriginalUrl}";
        await cache.RemoveCachedData(chaceKey);
    }
}