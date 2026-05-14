namespace ProductService.Models;

public class Bom
{
    public int BOMID { get; set; }
    public int ProductID { get; set; }
    public int ComponentID { get; set; }
    public decimal Quantity { get; set; }
    public string Version { get; set; } = "1.0";
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Product? Product { get; set; }
    public Product? Component { get; set; }
}
