using InventoryService.DTOs;
using InventoryService.Services.Interfaces;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/v1/suppliers")]
[Authorize]
public class SupplierController(ISupplierService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SupplierViewModel>>>> GetAll(
        [FromQuery] bool? isActive)
    {
        return Ok(await service.GetAllAsync(isActive));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierViewModel>>> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<SupplierViewModel>>> Create([FromBody] CreateSupplierRequest request)
    {
        var result = await service.CreateAsync(request);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.SupplierID }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<SupplierViewModel>>> Update(int id, [FromBody] UpdateSupplierRequest request)
    {
        var result = await service.UpdateAsync(id, request);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var result = await service.DeleteAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }
}
