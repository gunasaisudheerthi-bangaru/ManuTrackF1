using AnalyticsService.Data;
using AnalyticsService.Models;
using AnalyticsService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repositories;

public class ProductionMetricRepository(AnalyticsDbContext db) : IProductionMetricRepository
{
    // Change 3: pagination support
    public async Task<IEnumerable<ProductionMetric>> GetAllAsync(
        string? metricType = null,
        string? serviceSource = null,
        DateTime? from = null,
        DateTime? to = null,
        int page = 1,
        int pageSize = 50)
    {
        var query = BuildQuery(metricType, serviceSource, from, to);
        return await query
            .OrderByDescending(m => m.RecordedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // Change 3: total count for pagination metadata
    public async Task<int> CountAsync(
        string? metricType = null,
        string? serviceSource = null,
        DateTime? from = null,
        DateTime? to = null)
        => await BuildQuery(metricType, serviceSource, from, to).CountAsync();

    public async Task<ProductionMetric> CreateAsync(ProductionMetric metric)
    {
        db.ProductionMetrics.Add(metric);
        await db.SaveChangesAsync();
        return metric;
    }

    public async Task<Dictionary<string, decimal>> GetLatestKpisAsync()
    {
        return await db.ProductionMetrics
            .GroupBy(m => m.MetricName)
            .Select(g => new
            {
                g.Key,
                Value = g.OrderByDescending(m => m.RecordedDate).First().Value
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value);
    }

    public async Task<int> GetTotalCountAsync() =>
        await db.ProductionMetrics.CountAsync();

    // ── shared filter builder ──────────────────────────────────────────────────
    private IQueryable<ProductionMetric> BuildQuery(
        string? metricType, string? serviceSource, DateTime? from, DateTime? to)
    {
        var query = db.ProductionMetrics.AsQueryable();
        if (!string.IsNullOrWhiteSpace(metricType))
            query = query.Where(m => m.MetricType == metricType);
        if (!string.IsNullOrWhiteSpace(serviceSource))
            query = query.Where(m => m.ServiceSource == serviceSource);
        if (from.HasValue)
            query = query.Where(m => m.RecordedDate >= from.Value);
        if (to.HasValue)
            query = query.Where(m => m.RecordedDate <= to.Value);
        return query;
    }
}
