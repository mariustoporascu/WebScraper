using Application.Common.Interfaces;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.ScrapedUrls.EventHandlers;

public class ScrapedUrlCompletedEventHandler : INotificationHandler<ScrapedUrlCompletedEvent>
{
    private readonly ILogger<ScrapedUrlCompletedEventHandler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ScrapedUrlCompletedEventHandler(
        ILogger<ScrapedUrlCompletedEventHandler> logger,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Handle(
        ScrapedUrlCompletedEvent notification,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "WebScraperApi Domain Event: {DomainEvent}",
            notification.GetType().Name
        );
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        // Update the database
        dbContext.ScrapedUrls.Update(notification.Item);
        await dbContext.SaveChangesAsync(cancellationToken);

        // TODO: Add logic to send notification to client
    }
}
