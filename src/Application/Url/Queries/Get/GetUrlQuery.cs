using Application.DTOs;
using MediatR;

namespace Application.Url.Queries.Get;

public record GetUrlQuery(string shortUrl) : IRequest<GetUrlDto>;