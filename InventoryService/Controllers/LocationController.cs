using InventoryService.DTOs;
using InventoryService.Services.Interfaces;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/v1/locations")]
[Authorize]
public class LocationController(ILocationService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LocationViewModel>>>> GetAll(
        [FromQuery] bool? isActive)
    {
        return Ok(await service.GetAllAsync(isActive));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<LocationViewModel>>> GetById(int id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<LocationViewModel>>> Create(
        [FromBody] CreateLocationRequest request)
    {
        var result = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.LocationID }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<LocationViewModel>>> Update(
        int id, [FromBody] UpdateLocationRequest request)
    {
        return Ok(await service.UpdateAsync(id, request));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        return Ok(await service.DeleteAsync(id));
    }
}
