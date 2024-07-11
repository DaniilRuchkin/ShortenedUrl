using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Handlers;

public class DeleteUrlCommandHandler(ICommandUrlRepository commandUrlRepository, 
    IQueryUrlRepository queryUrlRepository, IPasswordHasher<string> passwordHasher) : IRequestHandler<DeleteUrlCommand>
{
    public async Task Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = await queryUrlRepository.GetUrlAsync(request.shortenedUrl, cancellationToken);

        if (entityToDelete == null) 
        { 
            throw new NullReferenceException();
        }

        var passwordVerificationPassword = passwordHasher.VerifyHashedPassword(null!, entityToDelete.Password, request.password);

        if (passwordVerificationPassword == PasswordVerificationResult.Success)
        {
            await commandUrlRepository.DeleteUrlAsync(entityToDelete, cancellationToken);
        }
    }
}