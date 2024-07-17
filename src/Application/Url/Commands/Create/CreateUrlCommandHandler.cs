using Application.DTOs;
using Application.Utilities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;

namespace Application.Url.Commands.Create;

public class CreateUrlCommandHandler(IPasswordHasher<string> passwordHasher, UrlDbContext context) : IRequestHandler<CreateUrlCommand, CreateDto>
{
    public async Task<CreateDto> Handle(CreateUrlCommand request, CancellationToken cancellationToken)
    {
        var hashedPassword = passwordHasher.HashPassword(null!, request.password);
        var shortenedUrl = ShortenedPathGenerator.GenerateShortenedPath();
        var cacheKeyShortUrl = $"{shortenedUrl}";

        var urlCreate = new Entity
        {
            OriginalUrl = request.urlOriginal,
            Password = hashedPassword,
            ShortenedUrl = shortenedUrl
        };

        await context.AddAsync(urlCreate, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var urlShort = new CreateDto
        {
            Url = shortenedUrl
        };

        return urlShort;
    }
}