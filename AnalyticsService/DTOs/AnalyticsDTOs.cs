using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.DTOs;

public class GenerateKpiReportRequest : IValidatableObject
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Report type is required.")]
    [RegularExpression("^(YieldRate|DefectRate|OnTimeCompletion|ProductionVolume|InventoryTurnover|Custom)$",
        ErrorMessage = "ReportType must be one of: YieldRate, DefectRate, OnTimeCompletion, ProductionVolume, InventoryTurnover, Custom.")]
    public string ReportType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Scope is required.")]
    [MinLength(3, ErrorMessage = "Scope must be at least 3 characters.")]
    [MaxLength(500, ErrorMessage = "Scope cannot exceed 500 characters.")]
    public string Scope { get; set; } = string.Empty;

    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }

    [MaxLength(8000, ErrorMessage = "Metrics JSON cannot exceed 8000 characters.")]
    public string Metrics { get; set; } = "{}";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PeriodStart.HasValue && PeriodEnd.HasValue && PeriodEnd.Value <= PeriodStart.Value)
            yield return new ValidationResult(
                "PeriodEnd must be after PeriodStart.",
                [nameof(PeriodEnd)]);
    }
}

public class RecordMetricRequest
{
    [Required(ErrorMessage = "Metric type is required.")]
    [RegularExpression("^(YieldRate|DefectRate|Throughput|Downtime|CycleTime|OEE|Custom)$",
        ErrorMessage = "MetricType must be one of: YieldRate, DefectRate, Throughput, Downtime, CycleTime, OEE, Custom.")]
    public string MetricType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Metric name is required.")]
    [MinLength(2, ErrorMessage = "Metric name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Metric name cannot exceed 200 characters.")]
    public string MetricName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Value is required.")]
    [Range(-999999999.9999, 999999999.9999, ErrorMessage = "Value is out of allowed range.")]
    public decimal Value { get; set; }

    [MaxLength(50, ErrorMessage = "Unit cannot exceed 50 characters.")]
    public string Unit { get; set; } = string.Empty;

    [RegularExpression(
        "^(AuthService|ProductService|WorkOrderService|InventoryService|QualityService|ComplianceService|AnalyticsService|NotificationService)?$",
        ErrorMessage = "ServiceSource must be a valid ManuTrack microservice name.")]
    [MaxLength(100, ErrorMessage = "ServiceSource cannot exceed 100 characters.")]
    public string? ServiceSource { get; set; }

    [MaxLength(100, ErrorMessage = "EntityID cannot exceed 100 characters.")]
    public string? EntityID { get; set; }
}

public class KpiReportViewModel
{
    public int ReportID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Metrics { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; }
    public string GeneratedBy { get; set; } = string.Empty;
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
}

public class ProductionMetricViewModel
{
    public int MetricID { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public string MetricName { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? ServiceSource { get; set; }
    public string? EntityID { get; set; }
    public DateTime RecordedDate { get; set; }
}

public class DashboardSummaryViewModel
{
    public int TotalReports { get; set; }
    public int TotalMetrics { get; set; }
    public Dictionary<string, decimal> LatestKpis { get; set; } = new();
}
