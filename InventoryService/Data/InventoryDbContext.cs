using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<InventoryItem>      InventoryItems      => Set<InventoryItem>();
    public DbSet<InventoryLocation>  InventoryLocations  => Set<InventoryLocation>();
    public DbSet<PurchaseOrder>      PurchaseOrders      => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem>  PurchaseOrderItems  => Set<PurchaseOrderItem>();
    public DbSet<Supplier>           Suppliers           => Set<Supplier>();
    public DbSet<StockMovement>      StockMovements      => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── InventoryLocation ──────────────────────────────────────────────────
        modelBuilder.Entity<InventoryLocation>(e =>
        {
            e.HasKey(l => l.LocationID);
            e.Property(l => l.LocationID).ValueGeneratedOnAdd();
            e.Property(l => l.Name).IsRequired().HasMaxLength(200);
            e.Property(l => l.Description).HasMaxLength(500);
            e.Property(l => l.IsActive).HasDefaultValue(true);
            e.HasIndex(l => l.Name).IsUnique();
            e.HasIndex(l => l.IsActive);
        });

        // ── InventoryItem ──────────────────────────────────────────────────────
        modelBuilder.Entity<InventoryItem>(e =>
        {
            e.HasKey(i => i.InventoryID);
            e.Property(i => i.InventoryID).ValueGeneratedOnAdd();
            e.Property(i => i.ItemType).IsRequired().HasMaxLength(20).HasDefaultValue("Product");
            e.Property(i => i.ProductID).IsRequired(false);
            e.Property(i => i.ComponentID).IsRequired(false);
            e.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            e.Property(i => i.LocationID).IsRequired(false);
            e.Property(i => i.QuantityOnHand).HasColumnType("decimal(18,4)");
            e.Property(i => i.MinimumQuantity).HasColumnType("decimal(18,4)");
            e.Property(i => i.Status).IsRequired().HasMaxLength(50).HasDefaultValue("InStock");
            e.Property(i => i.Notes).HasMaxLength(500);

            e.HasOne(i => i.Location)
             .WithMany(l => l.InventoryItems)
             .HasForeignKey(i => i.LocationID)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasMany(i => i.StockMovements)
             .WithOne(m => m.InventoryItem)
             .HasForeignKey(m => m.InventoryID)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(i => i.ProductID);
            e.HasIndex(i => i.LocationID);
            e.HasIndex(i => i.Status);
        });

        // ── Supplier ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Supplier>(e =>
        {
            e.HasKey(s => s.SupplierID);
            e.Property(s => s.SupplierID).ValueGeneratedOnAdd();
            e.Property(s => s.Name).IsRequired().HasMaxLength(200);
            e.Property(s => s.ContactPerson).HasMaxLength(200);
            e.Property(s => s.Phone).HasMaxLength(50);
            e.Property(s => s.Email).HasMaxLength(200);
            e.Property(s => s.Address).HasMaxLength(500);
            e.Property(s => s.IsActive).HasDefaultValue(true);
        });

        // ── PurchaseOrder ──────────────────────────────────────────────────────
        modelBuilder.Entity<PurchaseOrder>(e =>
        {
            e.HasKey(p => p.POID);
            e.Property(p => p.POID).ValueGeneratedOnAdd();
            e.Property(p => p.SupplierID).IsRequired().HasMaxLength(100).HasDefaultValue("");
            e.Property(p => p.SupplierName).IsRequired().HasMaxLength(200).HasDefaultValue("");
            e.Property(p => p.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            e.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            e.Property(p => p.Notes).HasMaxLength(1000);
            e.HasOne(p => p.Supplier)
             .WithMany(s => s.PurchaseOrders)
             .HasForeignKey(p => p.SupplierRefID)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ── PurchaseOrderItem ──────────────────────────────────────────────────
        modelBuilder.Entity<PurchaseOrderItem>(e =>
        {
            e.HasKey(i => i.POItemID);
            e.Property(i => i.POItemID).ValueGeneratedOnAdd();
            e.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
            e.Property(i => i.Quantity).HasColumnType("decimal(18,4)");
            e.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
            e.Property(i => i.TotalPrice).HasColumnType("decimal(18,2)");
            e.Property(i => i.ReceivedQty).HasColumnType("decimal(18,4)");
            e.HasOne(i => i.PurchaseOrder)
             .WithMany(p => p.Items)
             .HasForeignKey(i => i.POID)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.InventoryItem)
             .WithMany()
             .HasForeignKey(i => i.InventoryID)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── StockMovement ──────────────────────────────────────────────────────
        modelBuilder.Entity<StockMovement>(e =>
        {
            e.HasKey(m => m.MovementID);
            e.Property(m => m.MovementID).ValueGeneratedOnAdd();
            e.Property(m => m.MovementType).IsRequired().HasMaxLength(50);
            e.Property(m => m.Quantity).HasColumnType("decimal(18,4)");
            e.Property(m => m.Reason).IsRequired().HasMaxLength(500);
            e.Property(m => m.ReferenceID).HasMaxLength(100);
            e.HasIndex(m => m.InventoryID);
        });
    }
}
