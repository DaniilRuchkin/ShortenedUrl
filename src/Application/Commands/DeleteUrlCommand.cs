using MediatR;

namespace Application.Commands;

public record DeleteUrlCommand(string shortenedUrl, string password) : IRequest;