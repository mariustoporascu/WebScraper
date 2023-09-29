using System.Text.Json;
using Application.Common.Interfaces;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.ScrapedUrls.EventHandlers;

public class ScrapedUrlCreatedEventHandler : INotificationHandler<ScrapedUrlCreatedEvent>
{
    private readonly ILogger<ScrapedUrlCreatedEventHandler> _logger;
    private readonly IEnqueueScrapingJob _enqueueScrapingJob;

    public ScrapedUrlCreatedEventHandler(
        ILogger<ScrapedUrlCreatedEventHandler> logger,
        IEnqueueScrapingJob enqueueScrapingJob
    )
    {
        _logger = logger;
        _enqueueScrapingJob = enqueueScrapingJob;
    }

    public Task Handle(ScrapedUrlCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "WebScraperApi Domain Event: {DomainEvent}",
            notification.GetType().Name
        );

        _enqueueScrapingJob.EnqueueJob(JsonSerializer.Serialize(notification.Item), "task_queue");

        return Task.CompletedTask;
    }
}
