namespace ComplianceService.Models;

public class AuditEntry
{
    public int AuditID { get; set; }
    public int UserID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityID { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
