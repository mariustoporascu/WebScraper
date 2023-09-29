namespace Application.ScrapedUrls.Commands.CreateScrapedUrl;

public class CreateScrapedUrlCommandValidator : AbstractValidator<CreateScrapedUrlCommand>
{
    public CreateScrapedUrlCommandValidator()
    {
        RuleFor(v => v.Url)
            .NotEmpty();
    }
}
