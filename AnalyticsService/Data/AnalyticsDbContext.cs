using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Data;

public class AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : DbContext(options)
{
    public DbSet<KpiReport> KpiReports => Set<KpiReport>();
    public DbSet<ProductionMetric> ProductionMetrics => Set<ProductionMetric>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KpiReport>(e =>
        {
            e.HasKey(r => r.ReportID);
            e.Property(r => r.ReportID).ValueGeneratedOnAdd();
            e.Property(r => r.Title).IsRequired().HasMaxLength(200);
            e.Property(r => r.ReportType).IsRequired().HasMaxLength(100);
            e.Property(r => r.Scope).IsRequired().HasMaxLength(500);
            e.Property(r => r.Metrics).IsRequired().HasColumnType("nvarchar(max)");
            e.Property(r => r.GeneratedBy).IsRequired().HasMaxLength(200);
            e.HasIndex(r => r.ReportType);
            e.HasIndex(r => r.GeneratedDate);
        });

        modelBuilder.Entity<ProductionMetric>(e =>
        {
            e.HasKey(m => m.MetricID);
            e.Property(m => m.MetricID).ValueGeneratedOnAdd();
            e.Property(m => m.MetricType).IsRequired().HasMaxLength(100);
            e.Property(m => m.MetricName).IsRequired().HasMaxLength(200);
            e.Property(m => m.Value).HasColumnType("decimal(18,4)");
            e.Property(m => m.Unit).HasMaxLength(50);
            e.Property(m => m.ServiceSource).HasMaxLength(100);
            e.Property(m => m.EntityID).HasMaxLength(100);
            e.HasIndex(m => m.MetricType);
            e.HasIndex(m => m.RecordedDate);
        });
    }
}
