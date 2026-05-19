using ComplianceService.DTOs;
using ComplianceService.Services.Interfaces;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceService.Controllers;

[ApiController]
[Route("api/v1/compliance")]
[Authorize]
public class ComplianceReportController(IComplianceReportService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ComplianceReportViewModel>>>> GetAll(
        [FromQuery] string? status, [FromQuery] string? reportType)
        => Ok(await service.GetAllAsync(status, reportType));

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<ActionResult<ApiResponse<ComplianceReportViewModel>>> GetById(int id)
        => Ok(await service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<ActionResult<ApiResponse<ComplianceReportViewModel>>> Create(
        [FromBody] CreateComplianceReportRequest request)
    {
        // generatedBy is now extracted from JWT inside the service
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.ReportID }, result);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<ActionResult<ApiResponse<ComplianceReportViewModel>>> UpdateStatus(
        int id, [FromBody] UpdateReportStatusRequest request)
        => Ok(await service.UpdateStatusAsync(id, request));

    [HttpPut("{id:int}/approve")]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveReportRequest request)
        => Ok(await service.ApproveReportAsync(id, request));
}
