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
    Task<int> GetUnreadCountAsync(int userId);
}

