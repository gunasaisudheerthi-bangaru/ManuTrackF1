using NotificationService.Models;

namespace NotificationService.Repositories.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, string? status = null, string? category = null);
    Task<IEnumerable<Notification>> GetAllAsync(string? category = null, string? status = null);
    Task<Notification?> GetByIdAsync(int id);
    Task<Notification> CreateAsync(Notification notification);
    Task<IEnumerable<Notification>> CreateBulkAsync(IEnumerable<Notification> notifications);
    Task<Notification> UpdateAsync(Notification notification);

    // Change 4: grouped unread count by category
    Task<Dictionary<string, int>> GetUnreadCountByCategoryAsync(int userId);

    // Change 7: delete helpers
    Task<int> DeleteReadByUserAsync(int userId);
    Task<int> DeleteOlderThanAsync(DateTime cutoff);
}
