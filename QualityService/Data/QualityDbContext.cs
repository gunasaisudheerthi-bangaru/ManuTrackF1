using Microsoft.EntityFrameworkCore;
using QualityService.Models;

namespace QualityService.Data;

public class QualityDbContext(DbContextOptions<QualityDbContext> options) : DbContext(options)
{
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Defect> Defects => Set<Defect>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inspection>(e =>
        {
            e.HasKey(i => i.InspectionID);
            e.Property(i => i.InspectionID).ValueGeneratedOnAdd();
            e.Property(i => i.InspectorID).IsRequired().HasMaxLength(100);
            e.Property(i => i.InspectorName).IsRequired().HasMaxLength(200);
            e.Property(i => i.Result).HasMaxLength(50).HasDefaultValue(string.Empty);
            e.Property(i => i.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Scheduled");
            e.Property(i => i.Notes).HasMaxLength(1000);
            e.HasIndex(i => i.WorkOrderID);
            e.HasIndex(i => i.Status);
        });

        modelBuilder.Entity<Defect>(e =>
        {
            e.HasKey(d => d.DefectID);
            e.Property(d => d.DefectID).ValueGeneratedOnAdd();
            e.Property(d => d.Description).IsRequired().HasMaxLength(500);
            e.Property(d => d.Severity).IsRequired().HasMaxLength(50);
            e.Property(d => d.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Open");
            e.Property(d => d.Resolution).HasMaxLength(1000);

            e.HasOne(d => d.Inspection)
             .WithMany(i => i.Defects)
             .HasForeignKey(d => d.InspectionID)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(d => d.InspectionID);
            e.HasIndex(d => d.Status);
        });
    }
}
