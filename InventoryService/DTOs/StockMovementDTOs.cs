namespace InventoryService.DTOs;

public class StockMovementViewModel
{
    public int MovementID { get; set; }
    public int InventoryID { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ReferenceID { get; set; }
    public int PerformedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
