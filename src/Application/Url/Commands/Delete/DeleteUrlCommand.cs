using MediatR;

namespace URLShortener.Application.Url.Commands.Delete;

public record DeleteUrlCommand(string shortenedUrl, string password) : IRequest;