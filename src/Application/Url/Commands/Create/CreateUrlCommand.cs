using Application.DTOs;
using MediatR;

namespace Application.Url.Commands.Create;

public record CreateUrlCommand(string urlOriginal, string password) : IRequest<CreateDto>;