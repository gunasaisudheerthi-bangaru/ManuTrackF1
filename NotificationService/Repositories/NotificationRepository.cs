using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Enums;
using NotificationService.Models;
using NotificationService.Repositories.Interfaces;

namespace NotificationService.Repositories;

public class NotificationRepository(NotificationDbContext db) : INotificationRepository
{
    // Change 6: expiry filter helper
    private static readonly Func<Notification, bool> NotExpired =
        n => n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow;

    // Change 5 + 6: order by priority (Critical first) then date, filter expired
    public async Task<IEnumerable<Notification>> GetByUserIdAsync(
        int userId, string? status = null, string? category = null)
    {
        var query = db.Notifications
            .Where(n => n.UserID == userId
                     && (n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(n => n.Status == status);
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(n => n.Category == category);

        return await query
            .OrderBy(n => n.Priority == NotificationPriority.Critical ? 1
                        : n.Priority == NotificationPriority.High     ? 2
                        : n.Priority == NotificationPriority.Medium   ? 3 : 4)
            .ThenByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    // Change 6: filter expired in GetAll too
    public async Task<IEnumerable<Notification>> GetAllAsync(
        string? category = null, string? status = null)
    {
        var query = db.Notifications
            .Where(n => n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(n => n.Category == category);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(n => n.Status == status);

        return await query
            .OrderByDescending(n => n.CreatedDate)
            .Take(200)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id) =>
        await db.Notifications.FindAsync(id);

    public async Task<Notification> CreateAsync(Notification notification)
    {
        db.Notifications.Add(notification);
        await db.SaveChangesAsync();
        return notification;
    }

    public async Task<IEnumerable<Notification>> CreateBulkAsync(
        IEnumerable<Notification> notifications)
    {
        var list = notifications.ToList();
        db.Notifications.AddRange(list);
        await db.SaveChangesAsync();
        return list;
    }

    public async Task<Notification> UpdateAsync(Notification notification)
    {
        db.Notifications.Update(notification);
        await db.SaveChangesAsync();
        return notification;
    }

    // Change 4: grouped unread count by category
    public async Task<Dictionary<string, int>> GetUnreadCountByCategoryAsync(int userId)
    {
        var counts = await db.Notifications
            .Where(n => n.UserID == userId
                     && n.Status == NotificationStatus.Unread
                     && (n.ExpiryDate == null || n.ExpiryDate > DateTime.UtcNow))
            .GroupBy(n => n.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToListAsync();

        return counts.ToDictionary(x => x.Category, x => x.Count);
    }

    // Change 7: delete all read notifications for a user
    public async Task<int> DeleteReadByUserAsync(int userId)
    {
        var toDelete = await db.Notifications
            .Where(n => n.UserID == userId && n.Status == NotificationStatus.Read)
            .ToListAsync();

        db.Notifications.RemoveRange(toDelete);
        await db.SaveChangesAsync();
        return toDelete.Count;
    }

    // Change 7: delete notifications older than a cutoff date
    public async Task<int> DeleteOlderThanAsync(DateTime cutoff)
    {
        var toDelete = await db.Notifications
            .Where(n => n.CreatedDate < cutoff)
            .ToListAsync();

        db.Notifications.RemoveRange(toDelete);
        await db.SaveChangesAsync();
        return toDelete.Count;
    }
}
