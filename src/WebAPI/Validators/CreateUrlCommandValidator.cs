using FluentValidation;
using URLShortener.Application.Url.Commands.Create;

namespace URLShortener.Web.Validators;

public class CreateUrlCommandValidator : AbstractValidator<CreateUrlCommand>
{
    public CreateUrlCommandValidator()
    {
        RuleFor(x => x.urlOriginal)
            .NotEmpty().WithMessage("Original URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Invalid URL format.");

        RuleFor(x => x.password)
            .NotNull().WithMessage("Password is required.")
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
    }
}