namespace InventoryService.Models;

public class InventoryItem
{
    public int InventoryID { get; set; }
    public string ItemType { get; set; } = "Product";
    public int? ProductID { get; set; }
    public int? ComponentID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int? LocationID { get; set; }
    public InventoryLocation? Location { get; set; }
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public ICollection<StockMovement> StockMovements { get; set; } = [];
}
