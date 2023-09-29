namespace Domain.Entities;

public class ScrapedUrl : BaseAuditableEntity
{
    public string Url { get; set; } = null!;
    public string? JsonResult { get; set; }
    public bool ScrapeAllElements { get; set; }
    public bool IsScraped { get; set; }
}
