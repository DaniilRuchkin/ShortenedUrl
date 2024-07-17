using Application.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Configurations;
using Persistence.Data;

namespace Application.Url.Queries.Get;

public class GetUrlQueryHandler(UrlDbContext dbContext, IRedisCacheService redisCacheService, IOptions<CleanCacheSetting> options) : IRequestHandler<GetUrlQuery, GetUrlDto>
{
    public async Task<GetUrlDto> Handle(GetUrlQuery request, CancellationToken cancellationToken)
    {
        var result = await redisCacheService.GetCachedDataAsync<GetUrlDto>(request.shortUrl);

        if (result == null)
        {
            var entity = await dbContext.ShortUrl
            .AsNoTracking()
            .FirstOrDefaultAsync(url => url.ShortenedUrl == request.shortUrl, cancellationToken);

            var deleteBefore = options.Value.DeleteBeforeHours;

            await redisCacheService.SetCachedDataAsync(entity!.OriginalUrl!, request.shortUrl, deleteBefore);

            var originUrl = new GetUrlDto
            {
                Url = entity!.OriginalUrl
            };

            return originUrl;
        }

        return result;
    }
}