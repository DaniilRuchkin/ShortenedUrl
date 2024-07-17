using MediatR;

namespace Application.Url.Commands.Delete;

public record DeleteUrlCommand(string shortenedUrl, string password) : IRequest;