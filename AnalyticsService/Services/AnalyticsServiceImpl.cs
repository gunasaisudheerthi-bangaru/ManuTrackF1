using AnalyticsService.DTOs;
using AnalyticsService.Models;
using AnalyticsService.Repositories.Interfaces;
using AnalyticsService.Services.Interfaces;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace AnalyticsService.Services;

public class AnalyticsServiceImpl(IKpiReportRepository kpiRepo, IProductionMetricRepository metricRepo) : IAnalyticsService
{
    public async Task<ApiResponse<IEnumerable<KpiReportViewModel>>> GetAllReportsAsync(string? reportType)
    {
        var reports = await kpiRepo.GetAllAsync(reportType);
        return ApiResponse<IEnumerable<KpiReportViewModel>>.Ok(reports.Select(MapReport));
    }

    public async Task<ApiResponse<KpiReportViewModel>> GetReportByIdAsync(int id)
    {
        var report = await kpiRepo.GetByIdAsync(id) ?? throw new NotFoundException($"KPI report {id} not found.");
        return ApiResponse<KpiReportViewModel>.Ok(MapReport(report));
    }

    public async Task<ApiResponse<KpiReportViewModel>> GenerateReportAsync(GenerateKpiReportRequest request, string generatedBy)
    {
        var report = new KpiReport
        {
            Title = request.Title,
            ReportType = request.ReportType,
            Scope = request.Scope,
            Metrics = request.Metrics,
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            GeneratedBy = generatedBy,
            GeneratedDate = DateTime.UtcNow
        };

        var created = await kpiRepo.CreateAsync(report);
        return ApiResponse<KpiReportViewModel>.Ok(MapReport(created), "KPI report generated.");
    }

    public async Task<ApiResponse<IEnumerable<ProductionMetricViewModel>>> GetMetricsAsync(
        string? metricType, string? serviceSource, DateTime? from, DateTime? to)
    {
        var metrics = await metricRepo.GetAllAsync(metricType, serviceSource, from, to);
        return ApiResponse<IEnumerable<ProductionMetricViewModel>>.Ok(metrics.Select(MapMetric));
    }

    public async Task<ApiResponse<ProductionMetricViewModel>> RecordMetricAsync(RecordMetricRequest request)
    {
        var metric = new ProductionMetric
        {
            MetricType = request.MetricType,
            MetricName = request.MetricName,
            Value = request.Value,
            Unit = request.Unit,
            ServiceSource = request.ServiceSource,
            EntityID = request.EntityID,
            RecordedDate = DateTime.UtcNow
        };

        var created = await metricRepo.CreateAsync(metric);
        return ApiResponse<ProductionMetricViewModel>.Ok(MapMetric(created), "Metric recorded.");
    }

    public async Task<ApiResponse<DashboardSummaryViewModel>> GetDashboardSummaryAsync()
    {
        var reports = await kpiRepo.GetAllAsync();
        var metrics = await metricRepo.GetAllAsync();
        var latestKpis = await metricRepo.GetLatestKpisAsync();

        var summary = new DashboardSummaryViewModel
        {
            TotalReports = reports.Count(),
            TotalMetrics = metrics.Count(),
            LatestKpis = latestKpis
        };

        return ApiResponse<DashboardSummaryViewModel>.Ok(summary);
    }

    private static KpiReportViewModel MapReport(KpiReport r) => new()
    {
        ReportID = r.ReportID,
        Title = r.Title,
        ReportType = r.ReportType,
        Scope = r.Scope,
        Metrics = r.Metrics,
        GeneratedDate = r.GeneratedDate,
        GeneratedBy = r.GeneratedBy,
        PeriodStart = r.PeriodStart,
        PeriodEnd = r.PeriodEnd
    };

    private static ProductionMetricViewModel MapMetric(ProductionMetric m) => new()
    {
        MetricID = m.MetricID,
        MetricType = m.MetricType,
        MetricName = m.MetricName,
        Value = m.Value,
        Unit = m.Unit,
        ServiceSource = m.ServiceSource,
        EntityID = m.EntityID,
        RecordedDate = m.RecordedDate
    };
}
