using ManuTrack.SharedKernel.Helpers;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.DTOs;
using NotificationService.Services.Interfaces;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationController(INotificationService service) : ControllerBase
{
    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NotificationViewModel>>>> GetMine(
        [FromQuery] string? status, [FromQuery] string? category)
    {
        var userId = JwtHelper.GetUserId(User);
        return Ok(await service.GetForUserAsync(userId, status, category));
    }

    [HttpGet("my/unread-count")]
    public async Task<ActionResult<ApiResponse<UnreadCountViewModel>>> GetUnreadCount()
    {
        var userId = JwtHelper.GetUserId(User);
        return Ok(await service.GetUnreadCountAsync(userId));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NotificationViewModel>>>> GetAll(
        [FromQuery] string? category, [FromQuery] string? status)
    {
        return Ok(await service.GetAllAsync(category, status));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<NotificationViewModel>>> GetById(int id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Planner,InventoryManager")]
    public async Task<ActionResult<ApiResponse<NotificationViewModel>>> Send(
        [FromBody] SendNotificationRequest request)
    {
        var result = await service.SendAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.NotificationID }, result);
    }

    [HttpPost("broadcast")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NotificationViewModel>>>> Broadcast(
        [FromBody] BroadcastNotificationRequest request)
    {
        return Ok(await service.BroadcastAsync(request));
    }

    [HttpPut("{id:int}/read")]
    public async Task<ActionResult<ApiResponse<NotificationViewModel>>> MarkRead(int id)
    {
        return Ok(await service.MarkAsReadAsync(id));
    }

    [HttpPut("my/read-all")]
    public async Task<ActionResult<ApiResponse>> MarkAllRead()
    {
        var userId = JwtHelper.GetUserId(User);
        return Ok(await service.MarkAllAsReadAsync(userId));
    }

    [HttpDelete("my/read")]
    public async Task<ActionResult<ApiResponse>> DeleteMyRead()
    {
        var userId = JwtHelper.GetUserId(User);
        return Ok(await service.DeleteReadNotificationsAsync(userId));
    }

    [HttpDelete("cleanup")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> Cleanup()
    {
        return Ok(await service.CleanupOldNotificationsAsync());
    }
}
