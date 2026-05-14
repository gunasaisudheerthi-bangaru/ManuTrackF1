using AnalyticsService.Models;

namespace AnalyticsService.Repositories.Interfaces;

public interface IProductionMetricRepository
{
    Task<IEnumerable<ProductionMetric>> GetAllAsync(string? metricType = null, string? serviceSource = null, DateTime? from = null, DateTime? to = null);
    Task<ProductionMetric> CreateAsync(ProductionMetric metric);
    Task<Dictionary<string, decimal>> GetLatestKpisAsync();
}
