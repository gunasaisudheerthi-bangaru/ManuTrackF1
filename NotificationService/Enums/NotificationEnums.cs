namespace NotificationService.Enums;

public static class NotificationStatus
{
    public const string Unread = "Unread";
    public const string Read = "Read";
}

// Change 1: category constants (already existed — kept for completeness)
public static class NotificationCategory
{
    public const string WorkOrder  = "WorkOrder";
    public const string Inventory  = "Inventory";
    public const string Quality    = "Quality";
    public const string Compliance = "Compliance";
    public const string General    = "General";
}

// Change 5: priority constants
public static class NotificationPriority
{
    public const string Low      = "Low";
    public const string Medium   = "Medium";
    public const string High     = "High";
    public const string Critical = "Critical";
}
