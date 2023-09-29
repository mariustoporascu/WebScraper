using Application.Common.Models;
using Application.ScrapedUrls.Commands.CreateScrapedUrl;
using Application.ScrapedUrls.Queries.GetScrapedUrlsWithPagination;

namespace WebScraperApi.Endpoints;

public class Scrape : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .AllowAnonymous()
            .MapGet(GetScrapedUrlsWithPagination)
            .MapPost(CreateScrapedUrl);
    }

    public async Task<PaginatedList<ScrapedUrlBriefDto>> GetScrapedUrlsWithPagination(
        ISender sender,
        [AsParameters] GetScrapedUrlsWithPaginationQuery query
    )
    {
        return await sender.Send(query);
    }

    public async Task<int> CreateScrapedUrl(ISender sender, CreateScrapedUrlCommand command)
    {
        return await sender.Send(command);
    }
}
