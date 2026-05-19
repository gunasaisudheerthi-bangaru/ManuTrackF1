using ComplianceService.Models;

namespace ComplianceService.Repositories.Interfaces;

public interface IAuditRepository
{
    // Change 5: extended filters — entityType, action, entityId added
    Task<IEnumerable<AuditEntry>> GetAllAsync(
        string? userId = null,
        string? serviceName = null,
        DateTime? from = null,
        DateTime? to = null,
        string? entityType = null,
        string? action = null,
        string? entityId = null);

    Task<AuditEntry?> GetByIdAsync(int id);
    Task<AuditEntry> CreateAsync(AuditEntry entry);

    // Used by Change 6 (count gate) and Change 7 (metrics) — no Take limit
    Task<IEnumerable<AuditEntry>> GetAllForMetricsAsync(DateTime from, DateTime to);
}
