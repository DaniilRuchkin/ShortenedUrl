using URLShortener.Application.DTOs;
using MediatR;

namespace URLShortener.Application.Url.Commands.Create;

public record CreateUrlCommand(string urlOriginal, string password) : IRequest<CreateDto>;