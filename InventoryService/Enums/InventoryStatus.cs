namespace InventoryService.Enums;

public static class InventoryStatus
{
    public const string Available = "Available";
    public const string LowStock = "LowStock";
    public const string OutOfStock = "OutOfStock";
}

public static class PurchaseOrderStatus
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Delivered = "Delivered";
    public const string Cancelled = "Cancelled";
}
