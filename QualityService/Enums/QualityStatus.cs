namespace QualityService.Enums;

public static class InspectionStatus
{
    public const string Scheduled = "Scheduled";
    public const string InProgress = "InProgress";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
}

public static class InspectionResult
{
    public const string Pass = "Pass";
    public const string Fail = "Fail";
}

public static class DefectStatus
{
    public const string Open = "Open";
    public const string InProgress = "InProgress";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";
}
