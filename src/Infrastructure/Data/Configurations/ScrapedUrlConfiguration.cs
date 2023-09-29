using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ScrapedUrlConfiguration : IEntityTypeConfiguration<ScrapedUrl>
{
    public void Configure(EntityTypeBuilder<ScrapedUrl> builder)
    {
        builder.Property(t => t.Url).IsRequired();
    }
}
