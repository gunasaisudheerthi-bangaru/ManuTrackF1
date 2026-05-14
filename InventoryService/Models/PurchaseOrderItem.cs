namespace InventoryService.Models;

public class PurchaseOrderItem
{
    public int ItemID { get; set; }
    public int POID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public PurchaseOrder? PurchaseOrder { get; set; }
}
