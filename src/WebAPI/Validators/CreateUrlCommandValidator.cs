using URLShortener.Application.Url.Commands.Create;
using FluentValidation;

namespace URLShortener.Web.Validators;

public class CreateUrlCommandValidator : AbstractValidator<CreateUrlCommand>
{
    public CreateUrlCommandValidator()
    {
        RuleFor(x => x.urlOriginal).
            NotEmpty().WithMessage("URL is requered")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Invalid URL format.");

        RuleFor(x => x.password)
            .NotNull()
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(3).WithMessage("Password must be at leasta 3 long");
    }
}