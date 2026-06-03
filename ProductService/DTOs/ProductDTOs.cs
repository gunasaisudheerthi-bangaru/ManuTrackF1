using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Product name is required.")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [MinLength(2, ErrorMessage = "Category must be at least 2 characters.")]
    [MaxLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
    public string Category { get; set; } = string.Empty;

    [RegularExpression(@"^\d+\.\d+(\.\d+)?$", ErrorMessage = "Version must be in format like 1.0 or 1.0.0.")]
    [MaxLength(20, ErrorMessage = "Version cannot exceed 20 characters.")]
    public string Version { get; set; } = "1.0";

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }
}

public class UpdateProductRequest
{
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    public string? Name { get; set; }

    [MinLength(2, ErrorMessage = "Category must be at least 2 characters.")]
    [MaxLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
    public string? Category { get; set; }

    [RegularExpression(@"^\d+\.\d+(\.\d+)?$", ErrorMessage = "Version must be in format like 1.0 or 1.0.0.")]
    [MaxLength(20, ErrorMessage = "Version cannot exceed 20 characters.")]
    public string? Version { get; set; }

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }
}

public class UpdateProductStatusRequest
{
    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Draft|Active|Discontinued)$",
        ErrorMessage = "Status must be one of: Draft, Active, Discontinued.")]
    public string Status { get; set; } = string.Empty;
}

public class ProductViewModel
{
    public int ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}
