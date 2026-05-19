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
        => Ok(await service.GetAllAsync(isActive));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SupplierViewModel>>> GetById(int id)
        => Ok(await service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<SupplierViewModel>>> Create([FromBody] CreateSupplierRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.SupplierID }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<SupplierViewModel>>> Update(int id, [FromBody] UpdateSupplierRequest request)
        => Ok(await service.UpdateAsync(id, request));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
        => Ok(await service.DeleteAsync(id));
}
