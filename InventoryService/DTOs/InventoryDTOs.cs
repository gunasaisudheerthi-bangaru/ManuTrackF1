using System.ComponentModel.DataAnnotations;

namespace InventoryService.DTOs;

public class CreateInventoryItemRequest
{
    [Required(ErrorMessage = "ProductID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductID must be a positive integer.")]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LocationID is required.")]
    [MinLength(1, ErrorMessage = "LocationID cannot be empty.")]
    [MaxLength(100, ErrorMessage = "LocationID cannot exceed 100 characters.")]
    [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "LocationID may only contain letters, numbers, hyphens, and underscores.")]
    public string LocationID { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity on hand is required.")]
    [Range(0, 9999999.9999, ErrorMessage = "Quantity on hand must be between 0 and 9,999,999.")]
    public decimal QuantityOnHand { get; set; }

    [Range(0, 9999999.9999, ErrorMessage = "Minimum quantity must be between 0 and 9,999,999.")]
    public decimal MinimumQuantity { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}

public class UpdateInventoryItemRequest
{
    [MinLength(1, ErrorMessage = "LocationID cannot be empty.")]
    [MaxLength(100, ErrorMessage = "LocationID cannot exceed 100 characters.")]
    [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "LocationID may only contain letters, numbers, hyphens, and underscores.")]
    public string? LocationID { get; set; }

    [Range(0, 9999999.9999, ErrorMessage = "Quantity on hand must be between 0 and 9,999,999.")]
    public decimal? QuantityOnHand { get; set; }

    [Range(0, 9999999.9999, ErrorMessage = "Minimum quantity must be between 0 and 9,999,999.")]
    public decimal? MinimumQuantity { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}

public class AdjustQuantityRequest
{
    [Required(ErrorMessage = "Adjustment value is required.")]
    [Range(-9999999.9999, 9999999.9999, ErrorMessage = "Adjustment must be between -9,999,999 and 9,999,999.")]
    public decimal Adjustment { get; set; }

    [Required(ErrorMessage = "Reason for adjustment is required.")]
    [MinLength(5, ErrorMessage = "Reason must be at least 5 characters.")]
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
    public string Reason { get; set; } = string.Empty;
}

public class InventoryItemViewModel
{
    public int InventoryID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string LocationID { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
