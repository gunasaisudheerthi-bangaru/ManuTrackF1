using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QualityService.DTOs;
using QualityService.Services.Interfaces;

namespace QualityService.Controllers;

[ApiController]
[Route("api/v1/defects")]
[Authorize]
public class DefectController(IDefectService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DefectViewModel>>>> GetAll(
        [FromQuery] string? status, [FromQuery] string? severity)
    {
        return Ok(await service.GetAllAsync(status, severity));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<DefectViewModel>>> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("inspection/{inspectionId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<DefectViewModel>>>> GetByInspection(int inspectionId)
    {
        var result = await service.GetByInspectionIdAsync(inspectionId);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Inspector")]
    public async Task<ActionResult<ApiResponse<DefectViewModel>>> Create([FromBody] CreateDefectRequest request)
    {
        var result = await service.CreateAsync(request);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.DefectID }, result);
    }

    [HttpPut("{id:int}/resolve")]
    [Authorize(Roles = "Admin,Inspector,Operator")]
    public async Task<ActionResult<ApiResponse<DefectViewModel>>> Resolve(int id, [FromBody] ResolveDefectRequest request)
    {
        var result = await service.ResolveAsync(id, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,Inspector")]
    public async Task<ActionResult<ApiResponse<DefectViewModel>>> UpdateStatus(int id, [FromBody] UpdateDefectStatusRequest request)
    {
        var result = await service.UpdateStatusAsync(id, request);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }
}
