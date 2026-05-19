namespace InventoryService.Models;

public class InventoryLocation
{
    public int LocationID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }

    public ICollection<InventoryItem> InventoryItems { get; set; } = [];
}
