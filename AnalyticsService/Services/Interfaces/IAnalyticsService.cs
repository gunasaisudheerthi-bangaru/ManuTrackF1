using AnalyticsService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace AnalyticsService.Services.Interfaces;

public interface IAnalyticsService
{
    Task<ApiResponse<IEnumerable<KpiReportViewModel>>> GetAllReportsAsync(string? reportType);
    Task<ApiResponse<KpiReportViewModel>> GetReportByIdAsync(int id);
    Task<ApiResponse<KpiReportViewModel>> GenerateReportAsync(GenerateKpiReportRequest request, string generatedBy);
    Task<ApiResponse<IEnumerable<ProductionMetricViewModel>>> GetMetricsAsync(string? metricType, string? serviceSource, DateTime? from, DateTime? to);
    Task<ApiResponse<ProductionMetricViewModel>> RecordMetricAsync(RecordMetricRequest request);
    Task<ApiResponse<DashboardSummaryViewModel>> GetDashboardSummaryAsync();
}
