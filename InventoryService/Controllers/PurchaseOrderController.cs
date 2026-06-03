using InventoryService.DTOs;
using InventoryService.Services.Interfaces;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/v1/purchase-orders")]
[Authorize]
public class PurchaseOrderController(IPurchaseOrderService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseOrderViewModel>>>> GetAll([FromQuery] string? status)
    {
        return Ok(await service.GetAllAsync(status));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderViewModel>>> GetById(int id)
    {
        var result = await service.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderViewModel>>> Create([FromBody] CreatePurchaseOrderRequest request)
    {
        var result = await service.CreateAsync(request);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.POID }, result);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,InventoryManager")]
    public async Task<ActionResult<ApiResponse<PurchaseOrderViewModel>>> UpdateStatus(int id, [FromBody] UpdatePurchaseOrderStatusRequest request)
    {
        // Only Admin can approve a PO
        if (request.Status == "Approved" && !User.IsInRole("Admin"))
            return StatusCode(403, ApiResponse.Fail("Only Admin can approve purchase orders."));

        var result = await service.UpdateStatusAsync(id, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
