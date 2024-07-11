using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Commands.Handlers;

public class DeleteUrlCommandHandler(IPasswordHasher<string> passwordHasher, 
    UrlDbContext context) : IRequestHandler<DeleteUrlCommand>
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

        var passwordVerificationPassword = passwordHasher.VerifyHashedPassword(null!, entityToDelete.Password, request.password);

        if (passwordVerificationPassword == PasswordVerificationResult.Success)
        {
            context.ShortUrl.Remove(entityToDelete);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}