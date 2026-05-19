namespace ComplianceService.Models;

public class ComplianceReport
{
    public int ReportID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Metrics { get; set; } = "{}";
    public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
    public int GeneratedByUserID { get; set; }
    public string GeneratedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
}
