namespace AnalyticsService.Models;

public class KpiReport
{
    public int ReportID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Metrics { get; set; } = "{}";
    public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    public string GeneratedBy { get; set; } = string.Empty;
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
}
