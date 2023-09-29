using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;

namespace Application.ScrapedUrls.Queries.GetScrapedUrlsWithPagination;

public record GetScrapedUrlsWithPaginationQuery : IRequest<PaginatedList<ScrapedUrlBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetScrapedUrlsWithPaginationQueryHandler
    : IRequestHandler<GetScrapedUrlsWithPaginationQuery, PaginatedList<ScrapedUrlBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetScrapedUrlsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ScrapedUrlBriefDto>> Handle(
        GetScrapedUrlsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _context.ScrapedUrls
            .OrderBy(x => x.Id)
            .ProjectTo<ScrapedUrlBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
