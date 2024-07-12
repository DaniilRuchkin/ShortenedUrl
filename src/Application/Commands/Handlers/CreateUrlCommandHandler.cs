using Application.DTOs;
using Application.Interfaces;
using Application.Utilities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Persistence.Configurations;
using Persistence.Data;

namespace Application.Commands.Handlers;

public class CreateUrlCommandHandler(IPasswordHasher<string> passwordHasher,
    UrlDbContext context, IOptions<CleanCacheSetting> options,
    IRedisCacheService redisCache) : IRequestHandler<CreateUrlCommand, CreateDto>
{
    public async Task<CreateDto> Handle(CreateUrlCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"ShartenedUrl_{request.urlOriginal}";
        var cachedUrlShort = await redisCache.GetCachedDataAsync<CreateDto>(cacheKey);
        
        if (cachedUrlShort != null)
        {
            return cachedUrlShort;
        }

        var hashedPassword = passwordHasher.HashPassword(null!, request.password);
        var shortenedUrl = ShortenedPathGenerator.GenerateShortenedPath();

        var urlCreate = new Entity
        {
            OriginalUrl = request.urlOriginal,
            Password = hashedPassword,
            ShortenedUrl = shortenedUrl
        };

        await context.AddAsync(urlCreate, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var createShortenedUrl = new CreateDto
        {
            ShortenedUrl = shortenedUrl
        };

        var deleteBeforeHours = options.Value.DeleteBeforeHours;

        await redisCache.SetCachedDataAsync(cacheKey, createShortenedUrl, deleteBeforeHours);

        return createShortenedUrl;
    }
}