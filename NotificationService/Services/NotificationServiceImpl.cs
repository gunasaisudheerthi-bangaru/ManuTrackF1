using NotificationService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using NotificationService.DTOs;
using NotificationService.Models;
using NotificationService.DTOs;
using NotificationService.Repositories.Interfaces;
using NotificationService.Services.Interfaces;

namespace NotificationService.Services;

public class NotificationServiceImpl(INotificationRepository repo) : INotificationService
{
    public async Task<ApiResponse<IEnumerable<NotificationViewModel>>> GetForUserAsync(int userId, string? status, string? category)
    {
        var items = await repo.GetByUserIdAsync(userId, status, category);
        return ApiResponse<IEnumerable<NotificationViewModel>>.Ok(items.Select(Map));
    }

    public async Task<ApiResponse<IEnumerable<NotificationViewModel>>> GetAllAsync(string? category, string? status)
    {
        var items = await repo.GetAllAsync(category, status);
        return ApiResponse<IEnumerable<NotificationViewModel>>.Ok(items.Select(Map));
    }

    public async Task<ApiResponse<NotificationViewModel>> GetByIdAsync(int id)
    {
        var n = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Notification {id} not found.");
        return ApiResponse<NotificationViewModel>.Ok(Map(n));
    }

    public async Task<ApiResponse<NotificationViewModel>> SendAsync(SendNotificationRequest request)
    {
        var notification = new Notification
        {
            UserID = request.UserID,
            Title = request.Title,
            Message = request.Message,
            Category = request.Category,
            Status = NotificationStatus.Unread,
            CreatedDate = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(notification);
        return ApiResponse<NotificationViewModel>.Ok(Map(created), "Notification sent.");
    }

    public async Task<ApiResponse<IEnumerable<NotificationViewModel>>> BroadcastAsync(BroadcastNotificationRequest request)
    {
        var notifications = request.UserIDs.Select(uid => new Notification
        {
            UserID = uid,
            Title = request.Title,
            Message = request.Message,
            Category = request.Category,
            Status = NotificationStatus.Unread,
            CreatedDate = DateTime.UtcNow
        });

        var created = await repo.CreateBulkAsync(notifications);
        return ApiResponse<IEnumerable<NotificationViewModel>>.Ok(created.Select(Map), $"Broadcast sent to {request.UserIDs.Count} users.");
    }

    public async Task<ApiResponse<NotificationViewModel>> MarkAsReadAsync(int id)
    {
        var n = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Notification {id} not found.");
        n.Status = NotificationStatus.Read;
        n.ReadDate = DateTime.UtcNow;
        n.UpdatedDate = DateTime.UtcNow;
        var updated = await repo.UpdateAsync(n);
        return ApiResponse<NotificationViewModel>.Ok(Map(updated), "Notification marked as read.");
    }

    public async Task<ApiResponse> MarkAllAsReadAsync(int userId)
    {
        var items = await repo.GetByUserIdAsync(userId, NotificationStatus.Unread);
        foreach (var n in items)
        {
            n.Status = NotificationStatus.Read;
            n.ReadDate = DateTime.UtcNow;
            n.UpdatedDate = DateTime.UtcNow;
            await repo.UpdateAsync(n);
        }
        return ApiResponse.Ok("All notifications marked as read.");
    }

    public async Task<ApiResponse<int>> GetUnreadCountAsync(int userId)
    {
        var count = await repo.GetUnreadCountAsync(userId);
        return ApiResponse<int>.Ok(count);
    }

    private static NotificationViewModel Map(Notification n) => new()
    {
        NotificationID = n.NotificationID,
        UserID = n.UserID,
        Title = n.Title,
        Message = n.Message,
        Category = n.Category,
        Status = n.Status,
        CreatedDate = n.CreatedDate,
        UpdatedDate = n.UpdatedDate,
        ReadDate = n.ReadDate
    };
}

