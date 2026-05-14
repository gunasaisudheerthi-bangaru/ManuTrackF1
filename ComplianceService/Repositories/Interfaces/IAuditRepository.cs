using ComplianceService.Models;

namespace ComplianceService.Repositories.Interfaces;

public interface IAuditRepository
{
    Task<IEnumerable<AuditEntry>> GetAllAsync(string? userId = null, string? serviceName = null, DateTime? from = null, DateTime? to = null);
    Task<AuditEntry?> GetByIdAsync(int id);
    Task<AuditEntry> CreateAsync(AuditEntry entry);
}

