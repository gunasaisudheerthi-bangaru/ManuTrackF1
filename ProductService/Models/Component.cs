namespace ProductService.Models;

public class Component
{
    public int ComponentID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
