using Application.Url.Queries.Get;
using FluentValidation;

namespace Web.Validators;

public class GetUrlQueryValidator : AbstractValidator<GetUrlQuery>
{
    public GetUrlQueryValidator()
    {
        RuleFor(x => x.shortUrl).
            NotEmpty().WithMessage("URL is requered")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Invalid URL format.");
    }
}