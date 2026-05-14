using ComplianceService.Data;
using ComplianceService.Models;
using ComplianceService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComplianceService.Repositories;

public class AuditRepository(AuditDbContext db) : IAuditRepository
{
    public async Task<IEnumerable<AuditEntry>> GetAllAsync(string? userId = null, string? serviceName = null, DateTime? from = null, DateTime? to = null)
    {
        var query = db.AuditEntries.AsQueryable();
        if (!string.IsNullOrWhiteSpace(userId) && int.TryParse(userId, out var userIdInt))
            query = query.Where(a => a.UserID == userIdInt);
        if (!string.IsNullOrWhiteSpace(serviceName)) query = query.Where(a => a.ServiceName == serviceName);
        if (from.HasValue) query = query.Where(a => a.Timestamp >= from.Value);
        if (to.HasValue) query = query.Where(a => a.Timestamp <= to.Value);
        return await query.OrderByDescending(a => a.Timestamp).Take(1000).ToListAsync();
    }

    public async Task<AuditEntry?> GetByIdAsync(int id) => await db.AuditEntries.FindAsync(id);

    public async Task<AuditEntry> CreateAsync(AuditEntry entry)
    {
        db.AuditEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }
}

