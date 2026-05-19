namespace InventoryService.Models;

public class InventoryItem
{
    public int InventoryID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;

    // Changed from string to nullable int FK → InventoryLocation
    public int? LocationID { get; set; }
    public InventoryLocation? Location { get; set; }

    public decimal QuantityOnHand { get; set; }
    public decimal MinimumQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    public ICollection<StockMovement> StockMovements { get; set; } = [];
}
