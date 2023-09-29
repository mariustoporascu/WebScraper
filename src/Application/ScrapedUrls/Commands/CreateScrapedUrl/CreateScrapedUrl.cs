using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;

namespace Application.ScrapedUrls.Commands.CreateScrapedUrl;

internal class CreateScrapedUrl { }

public record CreateScrapedUrlCommand : IRequest<int>
{
    public string? Url { get; init; }
    public bool ScrapeAllElements { get; init; }
}

public class CreateScrapedUrlCommandHandler : IRequestHandler<CreateScrapedUrlCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public CreateScrapedUrlCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<int> Handle(
        CreateScrapedUrlCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new ScrapedUrl
        {
            Url = request.Url!,
            ScrapeAllElements = request.ScrapeAllElements,
            IsScraped = false
        };

        _context.ScrapedUrls.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new ScrapedUrlCreatedEvent(entity), cancellationToken);
        return entity.Id;
    }
}
