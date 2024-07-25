using URLShortener.Application.Url.Commands.Delete;
using FluentValidation;

namespace URLShortener.Web.Validators;

public class DeleteUrlCommandValidator : AbstractValidator<DeleteUrlCommand>
{
    public DeleteUrlCommandValidator()
    {
        RuleFor(x => x.shortenedUrl)
            .NotEmpty().WithMessage("URL is required");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("Password is required");
    }
}