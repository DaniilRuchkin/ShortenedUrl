using URLShortener.Application.DTOs;
using MediatR;

namespace URLShortener.Application.Url.Queries.Get;

public record GetUrlQuery(string shortUrl) : IRequest<GetUrlDto>;