using URLShortener.Application.DTOs;
using URLShortener.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NanoidDotNet;
using URLShortener.Persistence.Configurations;
using URLShortener.Persistence.Data;

namespace URLShortener.Application.Url.Commands.Create;

public class CreateUrlCommandHandler(IPasswordHasher<string> passwordHasher, UrlDbContext context, IOptions<CleanCacheSetting> options) : IRequestHandler<CreateUrlCommand, CreateDto>
{
    public async Task<CreateDto> Handle(CreateUrlCommand request, CancellationToken cancellationToken)
    {
        var hashedPassword = passwordHasher.HashPassword(null!, request.password);
        var size = options.Value.SizeIndificator;
        var shortenedUrl = Nanoid.Generate(size: size);

        var urlCreate = new ShortUrlsEntity
        {
            OriginalUrl = request.urlOriginal,
            Password = hashedPassword,
            ShortenedUrl = shortenedUrl
        };

        await context.AddAsync(urlCreate);
        await context.SaveChangesAsync();

        var createdUrl = new CreateDto
        {
            Url = shortenedUrl
        };

        return createdUrl;
    }
}