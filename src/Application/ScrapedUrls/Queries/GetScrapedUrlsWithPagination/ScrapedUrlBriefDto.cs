using Domain.Entities;

namespace Application.ScrapedUrls.Queries.GetScrapedUrlsWithPagination;

public class ScrapedUrlBriefDto
{
    public int Id { get; init; }

    public string? Url { get; init; }
    public bool ScrapeAllElements { get; init; }
    public string? JsonResult { get; init; }

    public bool IsScraped { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ScrapedUrl, ScrapedUrlBriefDto>();
        }
    }
}
