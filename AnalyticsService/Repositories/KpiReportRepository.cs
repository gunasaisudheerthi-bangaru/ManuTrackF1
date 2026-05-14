using AnalyticsService.Data;
using AnalyticsService.Models;
using AnalyticsService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repositories;

public class KpiReportRepository(AnalyticsDbContext db) : IKpiReportRepository
{
    public async Task<IEnumerable<KpiReport>> GetAllAsync(string? reportType = null)
    {
        var query = db.KpiReports.AsQueryable();
        if (!string.IsNullOrWhiteSpace(reportType)) query = query.Where(r => r.ReportType == reportType);
        return await query.OrderByDescending(r => r.GeneratedDate).ToListAsync();
    }

    public async Task<KpiReport?> GetByIdAsync(int id) => await db.KpiReports.FindAsync(id);

    public async Task<KpiReport> CreateAsync(KpiReport report)
    {
        db.KpiReports.Add(report);
        await db.SaveChangesAsync();
        return report;
    }
}
