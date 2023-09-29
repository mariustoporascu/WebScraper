using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ScrapedUrl> ScrapedUrls { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
