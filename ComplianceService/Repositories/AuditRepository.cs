using ComplianceService.Data;
using ComplianceService.Models;
using ComplianceService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceService.Repositories;

public class AuditRepository(AuditDbContext db) : IAuditRepository
{
    // Change 5: extended filters
    public async Task<IEnumerable<AuditEntry>> GetAllAsync(
        string? userId = null,
        string? serviceName = null,
        DateTime? from = null,
        DateTime? to = null,
        string? entityType = null,
        string? action = null,
        string? entityId = null)
    {
        var query = db.AuditEntries.AsQueryable();

        if (!string.IsNullOrWhiteSpace(userId) && int.TryParse(userId, out var userIdInt))
            query = query.Where(a => a.UserID == userIdInt);
        if (!string.IsNullOrWhiteSpace(serviceName))
            query = query.Where(a => a.ServiceName == serviceName);
        if (from.HasValue)
            query = query.Where(a => a.Timestamp >= from.Value);
        if (to.HasValue)
            query = query.Where(a => a.Timestamp <= to.Value);
        if (!string.IsNullOrWhiteSpace(entityType))
            query = query.Where(a => a.EntityType == entityType);
        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(a => a.Action.Contains(action));
        if (!string.IsNullOrWhiteSpace(entityId))
            query = query.Where(a => a.EntityID == entityId);

        return await query.OrderByDescending(a => a.Timestamp).Take(1000).ToListAsync();
    }

    public async Task<AuditEntry?> GetByIdAsync(int id) => await db.AuditEntries.FindAsync(id);

    public async Task<AuditEntry> CreateAsync(AuditEntry entry)
    {
        db.AuditEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    // Change 6 + 7: full period query with no Take limit
    public async Task<IEnumerable<AuditEntry>> GetAllForMetricsAsync(DateTime from, DateTime to)
    {
        return await db.AuditEntries
            .Where(a => a.Timestamp >= from && a.Timestamp <= to)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
}
