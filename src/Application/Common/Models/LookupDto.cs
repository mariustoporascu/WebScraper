using Domain.Entities;

namespace Application.Common.Models;

public class LookupDto
{
    public int Id { get; init; }

    public string? Url { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ScrapedUrl, LookupDto>();
        }
    }
}
