using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs;

public class CreateComponentRequest
{
    [Required(ErrorMessage = "Component name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Material type is required.")]
    [RegularExpression("^(RawMaterial|Part|SubAssembly|Chemical|Consumable)$",
        ErrorMessage = "MaterialType must be: RawMaterial, Part, SubAssembly, Chemical, or Consumable.")]
    public string MaterialType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unit is required.")]
    [RegularExpression("^(pcs|kg|metres|litres|grams|sheets|rolls)$",
        ErrorMessage = "Unit must be: pcs, kg, metres, litres, grams, sheets, or rolls.")]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}

public class ComponentViewModel
{
    public int ComponentID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
