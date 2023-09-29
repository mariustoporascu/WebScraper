namespace Application.Common.Interfaces;

public interface IEnqueueScrapingJob
{
    Task EnqueueJob(string message, string queue);
}
