namespace Domain.Events;

public class ScrapedUrlCreatedEvent : BaseEvent
{
    public ScrapedUrlCreatedEvent(ScrapedUrl item)
    {
        Item = item;
    }

    public ScrapedUrl Item { get; }
}
