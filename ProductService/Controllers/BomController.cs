using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Services.Interfaces;

namespace ProductService.Controllers;

[ApiController]
[Route("api/v1/bom")]
[Authorize]
public class BomController(IBomService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BomViewModel>>>> GetAll(
        [FromQuery] int? productId,
        [FromQuery] string? status)
    {
        var result = await service.GetAllBomsAsync(productId, status);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BomViewModel>>> GetById(int id)
    {
        var result = await service.GetBomByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("product/{productId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BomViewModel>>>> GetByProduct(int productId)
    {
        var result = await service.GetBomsByProductIdAsync(productId);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Planner")]
    public async Task<ActionResult<ApiResponse<BomViewModel>>> Create([FromBody] CreateBomRequest request)
    {
        var result = await service.CreateBomAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.BOMID }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Planner")]
    public async Task<ActionResult<ApiResponse<BomViewModel>>> Update(int id, [FromBody] UpdateBomRequest request)
    {
        var result = await service.UpdateBomAsync(id, request);
        return Ok(result);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<BomViewModel>>> UpdateStatus(int id, [FromBody] UpdateBomStatusRequest request)
    {
        var result = await service.UpdateBomStatusAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await service.DeleteBomAsync(id);
        return Ok(result);
    }
}
