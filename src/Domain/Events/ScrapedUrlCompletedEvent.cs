namespace Domain.Events;

public class ScrapedUrlCompletedEvent : BaseEvent
{
    public ScrapedUrlCompletedEvent(ScrapedUrl item)
    {
        Item = item;
    }

    public ScrapedUrl Item { get; }
}
