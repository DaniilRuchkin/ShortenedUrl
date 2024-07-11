using Application.DTOs;
using Application.Utilities;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Persistence.Configurations;
using Persistence.Data;

namespace Application.Commands.Handlers;

public class CreateUrlCommandHandler(IPasswordHasher<string> passwordHasher,
    IDistributedCache cache, UrlDbContext context, IOptions<CleanCacheSetting> options) : IRequestHandler<CreateUrlCommand, CreateDataDto>
{
    public async Task<CreateDataDto> Handle(CreateUrlCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"ShortenedUrl_{request.urlOriginal}";
        var cachedValue = await cache.GetStringAsync(cacheKey);

        if (cachedValue != null)
        {
            return JsonConvert.DeserializeObject<CreateDataDto>(cachedValue)!;
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

        var createShortenedUrl = new CreateDataDto
        {
            ShortenedUrl = shortenedUrl
        };

        var deleteBeforeHours = options.Value.DeleteBeforeHours;
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(deleteBeforeHours)
        };
        await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(createShortenedUrl), cacheOptions);

        return createShortenedUrl;
    }
}