using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record CreateUrlCommand(string urlOriginal, string password) : IRequest<CreateDataDto>;