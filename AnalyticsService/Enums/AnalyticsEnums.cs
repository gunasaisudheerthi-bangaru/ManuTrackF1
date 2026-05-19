namespace AnalyticsService.Enums;

// Change 1: KPI report type constants
public static class ReportType
{
    public const string YieldRate         = "YieldRate";
    public const string DefectRate        = "DefectRate";
    public const string OnTimeCompletion  = "OnTimeCompletion";
    public const string ProductionVolume  = "ProductionVolume";
    public const string InventoryTurnover = "InventoryTurnover";
    public const string Custom            = "Custom";
}

// Change 1: production metric type constants
public static class MetricType
{
    public const string YieldRate  = "YieldRate";
    public const string DefectRate = "DefectRate";
    public const string Throughput = "Throughput";
    public const string Downtime   = "Downtime";
    public const string CycleTime  = "CycleTime";
    public const string OEE        = "OEE";
    public const string Custom     = "Custom";
}
