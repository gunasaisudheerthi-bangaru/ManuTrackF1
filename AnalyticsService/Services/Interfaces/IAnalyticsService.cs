using AnalyticsService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace AnalyticsService.Services.Interfaces;

public interface IAnalyticsService
{
    Task<ApiResponse<IEnumerable<KpiReportViewModel>>> GetAllReportsAsync(string? reportType);
    Task<ApiResponse<KpiReportViewModel>> GetReportByIdAsync(int id);
    Task<ApiResponse<KpiReportViewModel>> GenerateReportAsync(GenerateKpiReportRequest request, string generatedBy);
    // Change 3: returns paged result
    Task<ApiResponse<PagedMetricsViewModel>> GetMetricsAsync(
        string? metricType, string? serviceSource, DateTime? from, DateTime? to,
        int page, int pageSize);
    Task<ApiResponse<ProductionMetricViewModel>> RecordMetricAsync(RecordMetricRequest request);
    // Change 4: enriched dashboard
    Task<ApiResponse<DashboardSummaryViewModel>> GetDashboardSummaryAsync();
}
