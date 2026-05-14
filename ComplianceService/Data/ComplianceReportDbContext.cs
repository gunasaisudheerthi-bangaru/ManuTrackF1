using ComplianceService.Models;
using Microsoft.EntityFrameworkCore;

namespace ComplianceService.Data;

public class ComplianceReportDbContext(DbContextOptions<ComplianceReportDbContext> options) : DbContext(options)
{
    public DbSet<ComplianceReport> ComplianceReports => Set<ComplianceReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComplianceReport>(e =>
        {
            e.HasKey(r => r.ReportID);
            e.Property(r => r.ReportID).ValueGeneratedOnAdd();
            e.Property(r => r.Title).IsRequired().HasMaxLength(200);
            e.Property(r => r.Scope).IsRequired().HasMaxLength(500);
            e.Property(r => r.Metrics).IsRequired().HasColumnType("nvarchar(max)");
            e.Property(r => r.GeneratedBy).IsRequired().HasMaxLength(200);
            e.Property(r => r.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Draft");
            e.Property(r => r.ReportType).IsRequired().HasMaxLength(100);
            e.HasIndex(r => r.ReportType);
            e.HasIndex(r => r.GeneratedDate);
        });
    }
}

