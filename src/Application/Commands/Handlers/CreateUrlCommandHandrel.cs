using Application.DTOs;
using Application.Utilities;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Commands.Handlers;

public class CreateUrlCommandHandrel(ICommandUrlRepository urlRepository, IPasswordHasher<string> passwordHasher, IDistributedCache cache) : IRequestHandler<CreateUrlCommand, CreateDataDto>
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
        await urlRepository.CreateUrlAsync(urlCreate, cancellationToken);

        var createShortenedUrl = new CreateDataDto
        {
            ShortenedUrl = shortenedUrl
        };

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(createShortenedUrl), cacheOptions);

        return createShortenedUrl;
    }
}