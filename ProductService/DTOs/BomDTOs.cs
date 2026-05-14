using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs;

public class CreateBomRequest
{
    [Required(ErrorMessage = "ProductID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductID must be a positive integer.")]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "ComponentID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ComponentID must be a positive integer.")]
    public int ComponentID { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.0001, 999999.9999, ErrorMessage = "Quantity must be between 0.0001 and 999,999.")]
    public decimal Quantity { get; set; }

    [RegularExpression(@"^\d+\.\d+(\.\d+)?$", ErrorMessage = "Version must be in format like 1.0 or 1.0.0.")]
    [MaxLength(20, ErrorMessage = "Version cannot exceed 20 characters.")]
    public string Version { get; set; } = "1.0";

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}

public class UpdateBomRequest
{
    [Range(0.0001, 999999.9999, ErrorMessage = "Quantity must be between 0.0001 and 999,999.")]
    public decimal? Quantity { get; set; }

    [RegularExpression(@"^\d+\.\d+(\.\d+)?$", ErrorMessage = "Version must be in format like 1.0 or 1.0.0.")]
    [MaxLength(20, ErrorMessage = "Version cannot exceed 20 characters.")]
    public string? Version { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}

public class UpdateBomStatusRequest
{
    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Active|Discontinued|Draft)$",
        ErrorMessage = "Status must be one of: Active, Discontinued, Draft.")]
    public string Status { get; set; } = string.Empty;
}

public class BomViewModel
{
    public int BOMID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int ComponentID { get; set; }
    public string ComponentName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}
