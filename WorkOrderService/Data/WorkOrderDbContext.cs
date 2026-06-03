using Microsoft.EntityFrameworkCore;
using WorkOrderService.Models;

namespace WorkOrderService.Data;

public class WorkOrderDbContext(DbContextOptions<WorkOrderDbContext> options) : DbContext(options)
{
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<WorkOrderTask> WorkOrderTasks => Set<WorkOrderTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkOrder>(e =>
        {
            e.HasKey(w => w.WorkOrderID);
            e.Property(w => w.WorkOrderID).ValueGeneratedOnAdd();
            e.Property(w => w.ProductName).IsRequired().HasMaxLength(200);
            e.Property(w => w.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            e.HasIndex(w => w.ProductID);
            e.HasIndex(w => w.Status);
        });

        modelBuilder.Entity<WorkOrderTask>(e =>
        {
            e.HasKey(t => t.TaskID);
            e.Property(t => t.TaskID).ValueGeneratedOnAdd();
            e.Property(t => t.Description).IsRequired().HasMaxLength(500);
            e.Property(t => t.AssignedTo).IsRequired().HasMaxLength(200);
            e.Property(t => t.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");

            e.HasOne(t => t.WorkOrder)
             .WithMany(w => w.Tasks)
             .HasForeignKey(t => t.WorkOrderID)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(t => t.WorkOrderID);
        });
    }
}
