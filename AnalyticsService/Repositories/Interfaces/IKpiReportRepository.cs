using AnalyticsService.Models;

namespace AnalyticsService.Repositories.Interfaces;

public interface IKpiReportRepository
{
    Task<IEnumerable<KpiReport>> GetAllAsync(string? reportType = null);
    Task<KpiReport?> GetByIdAsync(int id);
    Task<KpiReport> CreateAsync(KpiReport report);
}
