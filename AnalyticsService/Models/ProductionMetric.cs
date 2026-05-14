namespace AnalyticsService.Models;

public class ProductionMetric
{
    public int MetricID { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public string MetricName { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? ServiceSource { get; set; }
    public string? EntityID { get; set; }
    public DateTime RecordedDate { get; set; } = DateTime.UtcNow;
}
