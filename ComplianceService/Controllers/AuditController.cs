using ComplianceService.DTOs;
using ComplianceService.Services.Interfaces;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceService.Controllers;

[ApiController]
[Route("api/v1/audit")]
[Authorize]
public class AuditController(IAuditService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<ActionResult<ApiResponse<PagedAuditViewModel>>> GetAll(
        [FromQuery] string? userId,
        [FromQuery] string? serviceName,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? entityType,
        [FromQuery] string? action,
        [FromQuery] string? entityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await service.GetAllAsync(userId, serviceName, from, to, entityType, action, entityId, page, pageSize));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,ComplianceOfficer")]
    public async Task<ActionResult<ApiResponse<AuditEntryViewModel>>> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuditEntryViewModel>>> Log(
        [FromBody] LogAuditEntryRequest request)
    {
        var result = await service.LogAsync(request);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.AuditID }, result);
    }
}
