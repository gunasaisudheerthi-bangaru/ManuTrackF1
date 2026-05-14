using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Models;
using NotificationService.Repositories.Interfaces;

namespace NotificationService.Repositories;

public class NotificationRepository(NotificationDbContext db) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, string? status = null, string? category = null)
    {
        var query = db.Notifications.Where(n => n.UserID == userId);
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(n => n.Status == status);
        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(n => n.Category == category);
        return await query.OrderByDescending(n => n.CreatedDate).ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(string? category = null, string? status = null)
    {
        var query = db.Notifications.AsQueryable();
        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(n => n.Category == category);
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(n => n.Status == status);
        return await query.OrderByDescending(n => n.CreatedDate).Take(200).ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id) => await db.Notifications.FindAsync(id);

    public async Task<Notification> CreateAsync(Notification notification)
    {
        db.Notifications.Add(notification);
        await db.SaveChangesAsync();
        return notification;
    }

    public async Task<IEnumerable<Notification>> CreateBulkAsync(IEnumerable<Notification> notifications)
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

    public async Task<int> GetUnreadCountAsync(int userId) =>
        await db.Notifications.CountAsync(n => n.UserID == userId && n.Status == "Unread");
}

