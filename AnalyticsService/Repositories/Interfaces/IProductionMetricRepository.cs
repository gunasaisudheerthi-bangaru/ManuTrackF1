using AnalyticsService.Models;

namespace AnalyticsService.Repositories.Interfaces;

public interface IProductionMetricRepository
{
    // Change 3: page + pageSize added
    Task<IEnumerable<ProductionMetric>> GetAllAsync(
        string? metricType = null,
        string? serviceSource = null,
        DateTime? from = null,
        DateTime? to = null,
        int page = 1,
        int pageSize = 50);

    // Change 3: total count for pagination metadata
    Task<int> CountAsync(
        string? metricType = null,
        string? serviceSource = null,
        DateTime? from = null,
        DateTime? to = null);

    Task<ProductionMetric> CreateAsync(ProductionMetric metric);
    Task<Dictionary<string, decimal>> GetLatestKpisAsync();

    // Used by dashboard total count
    Task<int> GetTotalCountAsync();
}
