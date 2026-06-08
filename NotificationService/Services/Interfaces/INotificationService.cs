using ManuTrack.SharedKernel.Responses;
using NotificationService.DTOs;

namespace NotificationService.Services.Interfaces;

public interface INotificationService
{
    Task<ApiResponse<IEnumerable<NotificationViewModel>>> GetForUserAsync(int userId, string? status, string? category);
    Task<ApiResponse<IEnumerable<NotificationViewModel>>> GetAllAsync(string? category, string? status);
    Task<ApiResponse<NotificationViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<NotificationViewModel>> SendAsync(SendNotificationRequest request);
    Task<ApiResponse<IEnumerable<NotificationViewModel>>> BroadcastAsync(BroadcastNotificationRequest request);
    Task<ApiResponse<IEnumerable<NotificationViewModel>>> NotifyRoleAsync(NotifyRoleRequest request);
    Task<ApiResponse<NotificationViewModel>> MarkAsReadAsync(int id);
    Task<ApiResponse> MarkAllAsReadAsync(int userId);
    // Change 4: returns breakdown by category
    Task<ApiResponse<UnreadCountViewModel>> GetUnreadCountAsync(int userId);
    // Change 7: delete endpoints
    Task<ApiResponse> DeleteReadNotificationsAsync(int userId);
    Task<ApiResponse> CleanupOldNotificationsAsync();
}
