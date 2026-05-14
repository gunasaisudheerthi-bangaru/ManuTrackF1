using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>(e =>
        {
            e.HasKey(i => i.InventoryID);
            e.Property(i => i.InventoryID).ValueGeneratedOnAdd();
            e.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            e.Property(i => i.LocationID).IsRequired().HasMaxLength(100);
            e.Property(i => i.QuantityOnHand).HasColumnType("decimal(18,4)");
            e.Property(i => i.MinimumQuantity).HasColumnType("decimal(18,4)");
            e.Property(i => i.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Available");
            e.Property(i => i.Notes).HasMaxLength(500);
            e.HasIndex(i => i.ProductID);
            e.HasIndex(i => i.LocationID);
        });

        modelBuilder.Entity<PurchaseOrder>(e =>
        {
            e.HasKey(p => p.POID);
            e.Property(p => p.POID).ValueGeneratedOnAdd();
            e.Property(p => p.SupplierID).IsRequired().HasMaxLength(100);
            e.Property(p => p.SupplierName).IsRequired().HasMaxLength(200);
            e.Property(p => p.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            e.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            e.Property(p => p.Notes).HasMaxLength(1000);
        });

        modelBuilder.Entity<PurchaseOrderItem>(e =>
        {
            e.HasKey(i => i.ItemID);
            e.Property(i => i.ItemID).ValueGeneratedOnAdd();
            e.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            e.Property(i => i.Quantity).HasColumnType("decimal(18,4)");
            e.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");

            e.HasOne(i => i.PurchaseOrder)
             .WithMany(p => p.Items)
             .HasForeignKey(i => i.POID)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
