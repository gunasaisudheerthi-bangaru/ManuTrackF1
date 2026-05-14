using AnalyticsService.DTOs;
using AnalyticsService.Services.Interfaces;
using ManuTrack.SharedKernel.Helpers;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers;

[ApiController]
[Route("api/v1/analytics")]
[Authorize]
public class AnalyticsController(IAnalyticsService service) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryViewModel>>> GetDashboard()
        => Ok(await service.GetDashboardSummaryAsync());

    [HttpGet("reports")]
    public async Task<ActionResult<ApiResponse<IEnumerable<KpiReportViewModel>>>> GetReports([FromQuery] string? reportType)
        => Ok(await service.GetAllReportsAsync(reportType));

    [HttpGet("reports/{id:int}")]
    public async Task<ActionResult<ApiResponse<KpiReportViewModel>>> GetReportById(int id)
        => Ok(await service.GetReportByIdAsync(id));

    [HttpPost("reports")]
    [Authorize(Roles = "Admin,ComplianceOfficer,Planner")]
    public async Task<ActionResult<ApiResponse<KpiReportViewModel>>> GenerateReport([FromBody] GenerateKpiReportRequest request)
    {
        var generatedBy = JwtHelper.GetName(User) ?? JwtHelper.GetUserId(User).ToString();
        var result = await service.GenerateReportAsync(request, generatedBy);
        return CreatedAtAction(nameof(GetReportById), new { id = result.Data!.ReportID }, result);
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductionMetricViewModel>>>> GetMetrics(
        [FromQuery] string? metricType,
        [FromQuery] string? serviceSource,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
        => Ok(await service.GetMetricsAsync(metricType, serviceSource, from, to));

    [HttpPost("metrics")]
    public async Task<ActionResult<ApiResponse<ProductionMetricViewModel>>> RecordMetric([FromBody] RecordMetricRequest request)
        => Ok(await service.RecordMetricAsync(request));
}
