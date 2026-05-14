using Microsoft.EntityFrameworkCore;
using NotificationService.Models;

namespace NotificationService.Data;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(e =>
        {
            e.HasKey(n => n.NotificationID);
            e.Property(n => n.NotificationID).ValueGeneratedOnAdd();
            e.Property(n => n.UserID).IsRequired().HasMaxLength(100);
            e.Property(n => n.Title).IsRequired().HasMaxLength(200);
            e.Property(n => n.Message).IsRequired().HasMaxLength(2000);
            e.Property(n => n.Category).IsRequired().HasMaxLength(100);
            e.Property(n => n.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Unread");
            e.HasIndex(n => n.UserID);
            e.HasIndex(n => n.Status);
            e.HasIndex(n => n.Category);
            e.HasIndex(n => n.CreatedDate);
        });
    }
}
